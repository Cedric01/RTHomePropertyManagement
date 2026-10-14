using Microsoft.Extensions.FileProviders;

namespace RTHomePropertyManagement.Extensions;

public static class ImageStorageExtensions
{
    // ImageStorage:Provider = "Local" (default, for development) or "Gcs"
    // (production; also needs ImageStorage:GcsBucket).
    public static IServiceCollection AddImageStorage(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();

        var provider = config["ImageStorage:Provider"] ?? "Local";
        if (provider.Equals("Gcs", StringComparison.OrdinalIgnoreCase))
            services.AddSingleton<IImageStorage, GcsImageStorage>();
        else
            services.AddSingleton<IImageStorage, LocalImageStorage>();

        return services;
    }

    // Serves locally stored photos. Does nothing when photos live in GCS.
    public static WebApplication UseLocalImageFiles(this WebApplication app)
    {
        if (app.Services.GetRequiredService<IImageStorage>() is LocalImageStorage local)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(local.RootPath),
                RequestPath = LocalImageStorage.RequestPath
            });
        }

        return app;
    }
}
