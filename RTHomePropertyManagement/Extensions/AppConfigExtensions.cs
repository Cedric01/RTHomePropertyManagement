using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Extensions;

public static class AppConfigExtensions
{
    public static WebApplication ConfigureCORS(
        this WebApplication app,
        IConfiguration config)
    {
        // The Azure Static Web Apps origin is gone now that the frontend lives on
        // Firebase Hosting - swapped for the real rthome-836d6.web.app origin.
        app.UseCors(options =>
        options.WithOrigins("http://localhost:4200", "https://rthome-836d6.web.app")
        .AllowAnyMethod()
        .AllowAnyHeader());
        return app;
    }

    public static IServiceCollection AddAppConfig(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<JWT>(config.GetSection("JWT"));
        return services;
    }
}
