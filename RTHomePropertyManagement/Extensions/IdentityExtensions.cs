using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RTHomePropertyManagement.Extensions;

public static class IdentityExtensions
{
    // Custom claim namespace Auth0's post-login Action writes the user's
    // roles into (see https://homy-api/roles on both the id token and the
    // access token). Must match ROLES_CLAIM in the Angular app's auth.guard.ts.
    public const string RolesClaimType = "https://homy-api/roles";

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

        // "Agent" policy - anything only agents should be able to do (creating,
        // editing, or deleting property listings) is gated behind this, on top
        // of just being authenticated. Requires the Auth0 Action that copies
        // roles onto the access token to be deployed, or this claim will never
        // be present and every agent-only call will 403.
        services.AddAuthorizationBuilder()
            .AddPolicy("Agent", policy => policy.RequireClaim(RolesClaimType, "agent"));

        return services;
    }

    public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
