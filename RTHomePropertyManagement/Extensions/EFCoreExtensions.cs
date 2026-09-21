using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Extensions;

public static class EFCoreExtensions
{
    public static IServiceCollection InjectDbContext(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<RealEstateDbContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString("Postgres"),
                x => x.EnableRetryOnFailure());
        });

        return services;
    }
}
