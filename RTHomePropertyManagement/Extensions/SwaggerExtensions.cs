namespace RTHomePropertyManagement.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }


    public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // Ensure this path matches the document name above
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RT Home Property Management API v1");
            // Optional: keep default route '/swagger'. To serve at app root, set RoutePrefix = string.Empty;
            // c.RoutePrefix = string.Empty;
        });
        return app;
    }

}
