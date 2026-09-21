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

// Reads are public (anyone can browse/search properties, locations, price
// ranges, listing types and agents without logging in). Authorization is
// applied per-endpoint instead of on the whole group: see the "Agent" policy
// checks inside PropertyEndpoints/PriceRangeEndpoints/EstimateRequestEndpoints
// for what actually requires a token (property create/update/delete requires
// the "agent" role specifically; viewing submitted estimate requests requires
// it too; submitting one is a public lead-capture form and stays open).
app.MapGroup("/api").MapPropertyEndpoints();
app.MapGroup("/api").MapLocationEndpoints();
app.MapGroup("/api").MapPriceRangeEndpoints();
app.MapGroup("/api").MapListingTypeEndpoints();
app.MapGroup("/api").MapAgentEndpoints();
app.MapGroup("/api").MapEstimateRequestEndpoints();

app.Run();
