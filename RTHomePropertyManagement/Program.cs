using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Resources;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Repositories;
using System.Reflection;

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

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerExplorer(builder.Configuration)
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityAuth(builder.Configuration);

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPriceRangeRepository, PriceRangeRepository>();
builder.Services.AddScoped<IListingTypeRepository, ListingTypeRepository>();
builder.Services.AddScoped<IEstimateRequestRepository, EstimateRequestRepository>();




var app = builder.Build();

app.UseExceptionHandler();

app.ConfigureSwaggerExplorer(builder.Configuration)
   .ConfigureCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.MapControllers();

// All /api endpoints now require a valid Auth0 access token (see
// Extensions/IdentityExtensions.cs for the JWT bearer setup). Callers are
// currently: the RealEstate.Mcp server (via its own M2M client-credentials
// token) and you, via Swagger's "Authorize" button (real user login) - the
// Angular frontend's login flow is being wired up in a follow-up session.
app.MapGroup("/api")
    .RequireAuthorization()
    .MapPropertyEndpoints();
app.MapGroup("/api")
    .RequireAuthorization()
    .MapLocationEndpoints();
app.MapGroup("/api")
    .RequireAuthorization()
    .MapPriceRangeEndpoints();
app.MapGroup("/api")
    .RequireAuthorization()
    .MapListingTypeEndpoints();
app.MapGroup("/api")
    .RequireAuthorization()
    .MapAgentEndpoints();
app.MapGroup("/api")
    .RequireAuthorization()
    .MapEstimateRequestEndpoints();

app.Run();
