using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

namespace RTHomePropertyManagement.Controllers;

public static class EstimateRequestEndpoints
{
    public static IEndpointRouteBuilder MapEstimateRequestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/estimaterequests", ListEstimateRequests);
        app.MapPost("/estimaterequests", CreateEstimateRequest);

        return app;
    }

    public static async Task<IResult> ListEstimateRequests(
        IEstimateRequestRepository repository)
    {
        var requests = await repository.GetAllAsync();
        return Results.Ok(requests);
    }

    public static async Task<IResult> CreateEstimateRequest(
        IEstimateRequestRepository repository,
        EstimateRequestCreateDto dto)
    {
        var email = dto.Email?.Trim() ?? string.Empty;
        if (email.Length == 0 || !new EmailAddressAttribute().IsValid(email))
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["email"] = ["A valid email address is required."]
            });

        var created = await repository.CreateAsync(new EstimateRequest { Email = email });
        return Results.Ok(created);
    }
}
