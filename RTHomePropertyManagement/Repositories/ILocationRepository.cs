using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public interface ILocationRepository
{
    Task<List<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(int id);
}

