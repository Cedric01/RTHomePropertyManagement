using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

namespace RTHomePropertyManagement.Controllers;

public static class PropertyEndpoints
{
    public static IEndpointRouteBuilder MapPropertyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/properties", ListProperties);
        app.MapGet("/properties/{id}", GetPropertyById);
        app.MapPost("/properties", CreateProperty)
            .RequireAuthorization("Agent")
            .AddEndpointFilter<ValidationFilter<PropertyCreateDto>>();
        app.MapPut("/properties/{id}", UpdateProperty)
            .RequireAuthorization("Agent")
            .AddEndpointFilter<ValidationFilter<PropertyUpdateDto>>();
        app.MapDelete("/properties/{id}", DeleteProperty).RequireAuthorization("Agent");
        return app;
    }

    public static async Task<IResult> GetPropertyById(
        IPropertyRepository repository,
        int id)
    {
        var property = await repository.GetByIdAsync(id);
        if (property is null) return Results.NotFound();
        return Results.Ok(property.ToDto());
    }

    public static async Task<IResult> CreateProperty(
        IPropertyRepository repository,
        IReferenceDataValidator referenceValidator,
        ILogger<EndpointLogCategory> logger,
        PropertyCreateDto dto)
    {
        var invalidRefs = await referenceValidator.GetInvalidPropertyReferencesAsync(
            dto.LocationId, dto.ListingTypeId, dto.AgentId, dto.PriceRangeId);
        if (invalidRefs.Count > 0)
            return Results.ValidationProblem(BuildReferenceErrors(invalidRefs));

        var model = dto.ToModel();
        var created = await repository.CreateAsync(model);
        logger.LogInformation("Created property {PropertyId}", created.Id);

        var resultDto = created.ToDto();
        return Results.Ok(resultDto);
    }

    public static async Task<IResult> ListProperties(
        IPropertyRepository repository,
        HttpResponse response,
        int? locationId,
        int? listingTypeId,
        bool? isForRent,
        decimal? minPrice,
        decimal? maxPrice,
        int? minBedrooms,
        int? page,
        int? pageSize)
    {
        var filter = new PropertyFilter
        {
            LocationId = locationId,
            ListingTypeId = listingTypeId,
            IsForRent = isForRent,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            MinBedrooms = minBedrooms,
            Page = page,
            PageSize = pageSize
        };

        var (properties, totalCount) = await repository.GetAllAsync(filter);
        response.Headers["X-Total-Count"] = totalCount.ToString();

        var dtos = properties.Select(p => p.ToDto()).ToList();
        return Results.Ok(dtos);
    }

    public static async Task<IResult> UpdateProperty(
        IPropertyRepository repository,
        IReferenceDataValidator referenceValidator,
        ILogger<EndpointLogCategory> logger,
        int id,
        PropertyUpdateDto updatedDto)
    {
        // Option A: fetch the existing, apply updates, and pass to repository.UpdateAsync
        var existing = await repository.GetByIdAsync(id);
        if (existing is null) return Results.NotFound();

        var invalidRefs = await referenceValidator.GetInvalidPropertyReferencesAsync(
            updatedDto.LocationId, updatedDto.ListingTypeId, updatedDto.AgentId, updatedDto.PriceRangeId);
        if (invalidRefs.Count > 0)
            return Results.ValidationProblem(BuildReferenceErrors(invalidRefs));

        existing.ApplyUpdate(updatedDto);

        try
        {
            var updated = await repository.UpdateAsync(id, existing);
            if (updated is null) return Results.NotFound();

            logger.LogInformation("Updated property {PropertyId}", id);
            return Results.Ok(updated.ToDto());
        }
        catch (ConcurrencyConflictException ex)
        {
            logger.LogWarning("Concurrency conflict updating property {PropertyId}: {Message}", id, ex.Message);
            return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict, title: "Concurrency conflict");
        }
    }

    public static async Task<IResult> DeleteProperty(
        IPropertyRepository repository,
        ILogger<EndpointLogCategory> logger,
        int id)
    {
        var deleted = await repository.DeleteAsync(id);
        if (!deleted)
            return Results.NotFound();

        logger.LogInformation("Deleted property {PropertyId}", id);
        return Results.NoContent();
    }

    private static Dictionary<string, string[]> BuildReferenceErrors(List<string> invalidFields) =>
        invalidFields.ToDictionary(f => f, f => new[] { $"{f} does not reference an existing record." });
}
