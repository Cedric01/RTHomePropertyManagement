using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPriceRangeRepository, PriceRangeRepository>();
builder.Services.AddScoped<IListingTypeRepository, ListingTypeRepository>();




var app = builder.Build();

app.ConfigureSwaggerExplorer()
   .ConfigureCORS(builder.Configuration)
   .AddIdentityAuthMiddlewares();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapControllers();
app.MapGroup("/api")
   .MapIdentityApi<AppUser>();
app.MapGroup("/api")
   .MapIdentityUserEndpoints();
app.MapGroup("/api")
    .MapPropertyEndpoints();
app.MapGroup("/api")
    .MapLocationEndpoints();
app.MapGroup("/api")
    .MapPriceRangeEndpoints();
app.MapGroup("/api")
    .MapListingTypeEndpoints();
app.MapGroup("/api")
    .MapAgentEndpoints();

app.Run();
