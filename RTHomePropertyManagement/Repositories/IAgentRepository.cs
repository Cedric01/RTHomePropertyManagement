using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public interface IAgentRepository
{
    Task<List<Agent>> GetAllAsync();
    Task<Agent?> GetByIdAsync(int id);
}
