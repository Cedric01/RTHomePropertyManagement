using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly RealEstateDbContext _dbContext;

    public LocationRepository(RealEstateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Location>> GetAllAsync()
    {
        return await _dbContext.Locations
            .OrderBy(l => l.DisplayName)
            .ToListAsync();
    }

    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _dbContext.Locations.FindAsync(id);
    }
}
