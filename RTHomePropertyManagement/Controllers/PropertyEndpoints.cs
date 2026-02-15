using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

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

    public static async Task<IResult> CreateProperty(
        IPropertyRepository repository,
        PropertyCreateDto dto)
    {
        var model = dto.ToModel();
        var created = await repository.CreateAsync(model);
        var resultDto = created.ToDto();
        return Results.Ok(resultDto);
    }

    public static async Task<IResult> ListProperties(
        IPropertyRepository repository)
    {
        var properties = await repository.GetAllAsync();
        var dtos = properties.Select(p => p.ToDto()).ToList();
        return Results.Ok(dtos);
    }

    public static async Task<IResult> UpdateProperty(
        IPropertyRepository repository,
        int id,
        PropertyUpdateDto updatedDto)
    {
        // Option A: fetch the existing, apply updates, and pass to repository.UpdateAsync
        var existing = await repository.GetByIdAsync(id);
        if (existing is null) return Results.NotFound();

        existing.ApplyUpdate(updatedDto);
        var updated = await repository.UpdateAsync(id, existing);
        if (updated is null) return Results.NotFound();

        return Results.Ok(updated.ToDto());
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