using RTHomePropertyManagement.Repositories;

namespace RTHomePropertyManagement.Extensions;

// Endpoint filter for routes with an {id} property id: only the user who
// created the property may continue. Runs after authorization, so the caller is
// already a signed-in agent. Properties with no owner (created before
// ownership existed) can't be changed through the API.
public sealed class PropertyOwnerFilter : IEndpointFilter
{
    private readonly IPropertyRepository _repository;

    public PropertyOwnerFilter(IPropertyRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var http = context.HttpContext;

        if (!int.TryParse(http.GetRouteValue("id")?.ToString(), out var id))
            return Results.NotFound();

        var property = await _repository.GetByIdAsync(id);
        if (property is null)
            return Results.NotFound();

        var userId = http.User.GetUserId();
        if (userId is null || property.OwnerId != userId)
            return Results.Forbid();

        return await next(context);
    }
}
