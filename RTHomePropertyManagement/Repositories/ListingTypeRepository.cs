using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public class ListingTypeRepository : IListingTypeRepository
{
    private readonly RealEstateDbContext _dbContext;

    public ListingTypeRepository(RealEstateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ListingType>> GetAllAsync()
    {
        return await _dbContext.ListingTypes
            .OrderBy(lt => lt.Id)
            .ToListAsync();
    }

    public async Task<ListingType?> GetByIdAsync(int id)
    {
        return await _dbContext.ListingTypes.FindAsync(id);
    }
}
