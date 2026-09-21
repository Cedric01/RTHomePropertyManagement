using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertyManagement.Controllers;

public static class AgentEndpoints
{
    public static IEndpointRouteBuilder MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/agents", ListAgents);
        app.MapGet("/agents/{id}", GetAgentById);

        return app;
    }

    public static async Task<IResult> ListAgents(IAgentRepository repository)
    {
        var agents = await repository.GetAllAsync();
        return Results.Ok(agents.Select(a => a.ToDto()).ToList());
    }

    public static async Task<IResult> GetAgentById(IAgentRepository repository, int id)
    {
        var agent = await repository.GetByIdAsync(id);
        if (agent is null) return Results.NotFound();

        return Results.Ok(agent.ToDto());
    }
}
