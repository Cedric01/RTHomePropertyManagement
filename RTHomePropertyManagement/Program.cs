using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Resources;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var keyVaultUri = builder.Configuration["KeyVaultUri"];
    if (!string.IsNullOrEmpty(keyVaultUri))
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

var otelBuilder = builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: "RTHomePropertyManagement",
            serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0"
        ));

// Only wire up Azure Monitor if a connection string is actually configured -
// UseAzureMonitor throws on an empty/invalid connection string, which was
// crashing the app on startup on hosts (like Cloud Run) where App Insights
// isn't set up. Same "skip if not configured" pattern as Key Vault above.
var appInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    otelBuilder.UseAzureMonitor(options =>
    {
        options.ConnectionString = appInsightsConnectionString;
        options.SamplingRatio = builder.Configuration.GetValue<float>("OpenTelemetry:SamplingRatio", 1.0f);
    });
}

otelBuilder.WithTracing(tracing => tracing.AddSource("Npgsql"));

builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<RealEstateDbContext>();

// Public API is unauthenticated, so it's the obvious target for abuse -
// partition by client IP so each visitor gets their own budget rather than
// one shared bucket for the whole app. 100 requests/minute is generous for
// normal browsing/search use while still stopping a runaway script or scraper.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("api", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerExplorer(builder.Configuration)
                .InjectDbContext(builder.Configuration)
                .AddIdentityAuth(builder.Configuration);

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPriceRangeRepository, PriceRangeRepository>();
builder.Services.AddScoped<IListingTypeRepository, ListingTypeRepository>();
builder.Services.AddScoped<IEstimateRequestRepository, EstimateRequestRepository>();




var app = builder.Build();

app.UseExceptionHandler();

app.ConfigureSwaggerExplorer(builder.Configuration)
   .ConfigureScalarApiReference(builder.Configuration)
   .ConfigureCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.UseRateLimiter();

app.MapControllers();

// Unauthenticated, not rate-limited - lets Cloud Run and uptime monitors
// probe liveness without burning into the "api" rate limit budget above.
app.MapHealthChecks("/healthz");

// Reads are public (anyone can browse/search properties, locations, price
// ranges, listing types and agents without logging in). Authorization is
// applied per-endpoint instead of on the whole group: see the "Agent" policy
// checks inside PropertyEndpoints/PriceRangeEndpoints/EstimateRequestEndpoints
// for what actually requires a token (property create/update/delete requires
// the "agent" role specifically; viewing submitted estimate requests requires
// it too; submitting one is a public lead-capture form and stays open).
var api = app.MapGroup("/api").RequireRateLimiting("api");
api.MapPropertyEndpoints();
api.MapLocationEndpoints();
api.MapPriceRangeEndpoints();
api.MapListingTypeEndpoints();
api.MapAgentEndpoints();
api.MapEstimateRequestEndpoints();

app.Run();
