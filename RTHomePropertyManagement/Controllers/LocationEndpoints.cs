using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

namespace RTHomePropertyManagement.Controllers;

public static class LocationEndpoints
{
    public static IEndpointRouteBuilder MapLocationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getlocations", ListLocations);
        app.MapGet("/locations/{id}", GetLocationById);

        return app;
    }

    public static async Task<IResult> ListLocations(
        ILocationRepository repository)
    {
        var locations = await repository.GetAllAsync();
        return Results.Ok(locations);
    }

    public static async Task<IResult> GetLocationById(
        ILocationRepository repository,
        int id)
    {
        var location = await repository.GetByIdAsync(id);
        if (location is null)
            return Results.NotFound();

        return Results.Ok(location);
    }
}

