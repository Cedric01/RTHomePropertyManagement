using Microsoft.OpenApi.Models;

namespace RTHomePropertyManagement.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services, IConfiguration config)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var domain = config["Auth0:Domain"];

            c.AddSecurityDefinition("Auth0", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"https://{domain}/authorize"),
                        TokenUrl = new Uri($"https://{domain}/oauth/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "Sign you in" },
                            { "profile", "Read your profile" }
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Auth0" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }


    public static WebApplication ConfigureSwaggerExplorer(this WebApplication app, IConfiguration config)
    {
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // Ensure this path matches the document name above
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RT Home Property Management API v1");
            // Optional: keep default route '/swagger'. To serve at app root, set RoutePrefix = string.Empty;
            // c.RoutePrefix = string.Empty;

            c.OAuthClientId(config["Auth0:SwaggerClientId"]);
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
            // Auth0 only issues a proper API access token (instead of just an
            // OIDC id_token) when "audience" is passed through on the
            // /authorize request - Swagger UI doesn't send it by default.
            c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
            {
                { "audience", config["Auth0:Audience"] ?? string.Empty }
            });
        });
        return app;
    }

}
