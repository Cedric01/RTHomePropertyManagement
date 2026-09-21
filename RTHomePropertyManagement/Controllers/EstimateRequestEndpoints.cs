using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RTHomePropertManagementTests")]

namespace RTHomePropertyManagement.Controllers;

public static class EstimateRequestEndpoints
{
    public static IEndpointRouteBuilder MapEstimateRequestEndpoints(this IEndpointRouteBuilder app)
    {
        // Submitting a request is a public lead-capture form - anyone browsing
        // the site can ask for a valuation without logging in. Viewing the
        // submitted requests is an agent-only function.
        app.MapGet("/estimaterequests", ListEstimateRequests).RequireAuthorization("Agent");
        app.MapPost("/estimaterequests", CreateEstimateRequest)
            .AddEndpointFilter<ValidationFilter<EstimateRequestCreateDto>>();

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
        ILogger<EndpointLogCategory> logger,
        EstimateRequestCreateDto dto)
    {
        var email = dto.Email.Trim();
        var created = await repository.CreateAsync(new EstimateRequest { Email = email });
        logger.LogInformation("Estimate request {EstimateRequestId} submitted", created.Id);
        return Results.Ok(created);
    }
}
