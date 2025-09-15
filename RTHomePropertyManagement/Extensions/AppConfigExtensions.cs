using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Extensions;

public static class AppConfigExtensions
{
    public static WebApplication ConfigureCORS(
        this WebApplication app,
        IConfiguration config)
    {
        app.UseCors(options =>
        options.WithOrigins("http://localhost:4200")
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
