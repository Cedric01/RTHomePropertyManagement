using Microsoft.AspNetCore.Mvc;
using RTHomePropertyManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace RTHomePropertyManagement.Controllers;

public static class PropertyEndpoints
{
    public static IEndpointRouteBuilder MapPropertyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/createproperty", CreateProperty);
        app.MapGet("/getproperties", ListProperties);
        app.MapPut("/properties/{id}", UpdateProperty);
        app.MapDelete("/properties/{id}", DeleteProperty);
        return app;
    }

    private static async Task<IResult> CreateProperty(
        [FromServices] AppDbContext dbContext,
        [FromBody] Property property)
    {
        dbContext.Add(property);
        await dbContext.SaveChangesAsync();
        return Results.Ok(property);
    }

    private static async Task<IResult> ListProperties(
        [FromServices] AppDbContext dbContext)
    {
        var properties = await dbContext.Properties.ToListAsync();
        return Results.Ok(properties);
    }

    private static async Task<IResult> UpdateProperty(
        [FromServices] AppDbContext dbContext,
        [FromRoute] int id,
        [FromBody] Property updatedProperty)
    {
        var property = await dbContext.Properties.FindAsync(id);
        if (property is null)
            return Results.NotFound();

        property = property with
        {
            Title = updatedProperty.Title,
            Description = updatedProperty.Description,
            Price = updatedProperty.Price,
            Bedrooms = updatedProperty.Bedrooms,
            Bathrooms = updatedProperty.Bathrooms
        };

        dbContext.Entry(property).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return Results.Ok(property);
    }

    private static async Task<IResult> DeleteProperty(
        [FromServices] AppDbContext dbContext,
        [FromRoute] int id)
    {
        var property = await dbContext.Properties.FindAsync(id);
        if (property is null)
            return Results.NotFound();

        dbContext.Properties.Remove(property);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
}