using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RTHomePropertyManagement.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityAuth(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{config["Auth0:Domain"]}/";
                options.Audience = config["Auth0:Audience"];
            });
        return services;
    }

    public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
