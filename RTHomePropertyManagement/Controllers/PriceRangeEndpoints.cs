using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

namespace RTHomePropertyManagement.Controllers;

public static class PriceRangeEndpoints
{
    public static IEndpointRouteBuilder MapPriceRangeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/priceranges", ListPriceRanges);
        app.MapPost("/priceranges", CreatePriceRange)
            .RequireAuthorization("Agent")
            .AddEndpointFilter<ValidationFilter<PriceRange>>();

        return app;
    }

    public static async Task<IResult> ListPriceRanges(
        IPriceRangeRepository repository)
    {
        var ranges = await repository.GetAllAsync();
        return Results.Ok(ranges);
    }

    public static async Task<IResult> CreatePriceRange(
        IPriceRangeRepository repository,
        ILogger<EndpointLogCategory> logger,
        PriceRange priceRange)
    {
        var created = await repository.CreateAsync(priceRange);
        logger.LogInformation("Created price range {PriceRangeId}", created.Id);
        return Results.Ok(created);
    }
}
