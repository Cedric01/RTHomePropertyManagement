using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Controllers;

public static class AgentEndpoints
{
    public static IEndpointRouteBuilder MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/agents", ListAgents);
        app.MapGet("/agents/{id}", GetAgentById);

        return app;
    }

    public static async Task<IResult> ListAgents(RealEstateDbContext db)
    {
        var agents = await db.Agents
            .OrderBy(a => a.Id)
            .Select(a => new
            {
                a.Id,
                a.Name,
                a.Designation,
                imageUrl = a.ImageUrl,
                link = a.ProfileLink ?? "/agent_details"
            })
            .ToListAsync();

        return Results.Ok(agents);
    }

    public static async Task<IResult> GetAgentById(RealEstateDbContext db, int id)
    {
        var agent = await db.Agents.FindAsync(id);
        if (agent is null) return Results.NotFound();
        return Results.Ok(agent);
    }
}