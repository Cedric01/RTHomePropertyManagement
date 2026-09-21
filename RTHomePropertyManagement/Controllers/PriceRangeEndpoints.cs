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
        app.MapPost("/priceranges", CreatePriceRange).RequireAuthorization("Agent");

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
        PriceRange priceRange)
    {
        var created = await repository.CreateAsync(priceRange);
        return Results.Ok(created);
    }
}
