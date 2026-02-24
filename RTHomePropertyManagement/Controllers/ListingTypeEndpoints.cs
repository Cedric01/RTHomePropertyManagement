using RTHomePropertyManagement.Repositories;

namespace RTHomePropertyManagement.Controllers;

public static class ListingTypeEndpoints
{
    public static IEndpointRouteBuilder MapListingTypeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/listingtypes", ListListingTypes);
        app.MapGet("/listingtypes/{id}", GetListingTypeById);

        return app;
    }

    public static async Task<IResult> ListListingTypes(
        IListingTypeRepository repository)
    {
        var listingTypes = await repository.GetAllAsync();
        return Results.Ok(listingTypes);
    }

    public static async Task<IResult> GetListingTypeById(
        IListingTypeRepository repository,
        int id)
    {
        var listingType = await repository.GetByIdAsync(id);
        if (listingType is null)
            return Results.NotFound();

        return Results.Ok(listingType);
    }
}
