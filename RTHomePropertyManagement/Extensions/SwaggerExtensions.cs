using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

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
        // Keep Swashbuckle's OpenAPI JSON generation (Scalar reads this doc
        // for its UI below) but drop Swagger's own UI - Scalar replaces it.
        app.UseSwagger();
        return app;
    }

    // Scalar's modern API reference UI, served at /scalar, reading the
    // OpenAPI document Swashbuckle generates above. Carries over the same
    // Auth0 authorization-code + PKCE flow (and the "audience" query param
    // Auth0 needs to issue an API access token) that Swagger UI used.
    public static WebApplication ConfigureScalarApiReference(this WebApplication app, IConfiguration config)
    {
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("RT Home Property Management API")
                .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json")
                .AddPreferredSecuritySchemes("Auth0")
                .AddAuthorizationCodeFlow("Auth0", flow =>
                {
                    flow.ClientId = config["Auth0:SwaggerClientId"];
                    flow.Pkce = Pkce.Sha256;
                    flow.AddQueryParameter("audience", config["Auth0:Audience"] ?? string.Empty);
                });
        });
        return app;
    }

}
