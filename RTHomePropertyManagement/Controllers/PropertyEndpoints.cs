using RTHomePropertyManagement.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

namespace RTHomePropertyManagement.Controllers;

public static class PropertyEndpoints
{
    public static IEndpointRouteBuilder MapPropertyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/createproperty", CreateProperty);
        app.MapGet("/getproperties", ListProperties);
        app.MapGet("/listingproperties", ListingProperties);
        app.MapPut("/properties/{id}", UpdateProperty);
        app.MapDelete("/properties/{id}", DeleteProperty);
        return app;
    }

    public static async Task<IResult> CreateProperty(
        IPropertyRepository repository,
        Property property)
    {
        var created = await repository.CreateAsync(property);
        return Results.Ok(created);
    }

    public static async Task<IResult> ListProperties(
        IPropertyRepository repository)
    {
        var properties = await repository.GetAllAsync();
        return Results.Ok(properties);
    }

    public static async Task<IResult> ListingProperties(
    IPropertyRepository repository)
    {
        var properties = await repository.GetAllListingPropertiesAsync();
        return Results.Ok(properties);
    }

    public static async Task<IResult> UpdateProperty(
        IPropertyRepository repository,
        int id,
        Property updatedProperty)
    {
        var updated = await repository.UpdateAsync(id, updatedProperty);
        if (updated is null)
            return Results.NotFound();

        return Results.Ok(updated);
    }

    public static async Task<IResult> DeleteProperty(
        IPropertyRepository repository,
        int id)
    {
        var deleted = await repository.DeleteAsync(id);
        if (!deleted)
            return Results.NotFound();

        return Results.NoContent();
    }
}