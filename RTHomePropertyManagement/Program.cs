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
builder.Services.AddScoped<IReferenceDataValidator, ReferenceDataValidator>();
builder.Services.AddScoped<IAgentRepository, AgentRepository>();




var app = builder.Build();

app.UseExceptionHandler();

app.ConfigureSwaggerExplorer(builder.Configuration)
   .ConfigureScalarApiReference(builder.Configuration)
   .ConfigureCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.UseRateLimiter();
app.MapGet("/", () => Results.Redirect("/scalar"));

app.MapControllers();
app.MapHealthChecks("/healthz");

static void MapApiEndpoints(RouteGroupBuilder group)
{
    group.MapPropertyEndpoints();
    group.MapLocationEndpoints();
    group.MapPriceRangeEndpoints();
    group.MapListingTypeEndpoints();
    group.MapAgentEndpoints();
    group.MapEstimateRequestEndpoints();
}

static async ValueTask<object?> WithApiVersionHeader(
    EndpointFilterInvocationContext invocationContext,
    EndpointFilterDelegate next)
{
    invocationContext.HttpContext.Response.Headers["X-Api-Version"] = "1.0";
    return await next(invocationContext);
}

var apiV1 = app.MapGroup("/api/v1").RequireRateLimiting("api");
MapApiEndpoints(apiV1);
apiV1.AddEndpointFilter(WithApiVersionHeader);

// Unversioned /api is a temporary alias for v1, kept only so the existing
// Angular frontend (which still calls /api/... directly) doesn't break.
// Remove this block once the frontend is updated to call /api/v1 - from
// that point /api should 404 rather than silently keep serving v1 forever.
var apiUnversioned = app.MapGroup("/api").RequireRateLimiting("api");
MapApiEndpoints(apiUnversioned);
apiUnversioned.AddEndpointFilter(WithApiVersionHeader);

app.Run();
