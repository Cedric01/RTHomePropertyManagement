using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly RealEstateDbContext _dbContext;

    public AgentRepository(RealEstateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Agent>> GetAllAsync()
    {
        return await _dbContext.Agents
            .OrderBy(a => a.Id)
            .ToListAsync();
    }

    public async Task<Agent?> GetByIdAsync(int id)
    {
        return await _dbContext.Agents.FindAsync(id);
    }
}
