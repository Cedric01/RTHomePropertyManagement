using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public class PriceRangeRepository : IPriceRangeRepository
{
    private readonly RealEstateDbContext _dbContext;

    public PriceRangeRepository(RealEstateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PriceRange>> GetAllAsync()
    {
        return await _dbContext.PriceRanges
            .OrderBy(p => p.MinPrice)
            .ToListAsync();
    }

    public async Task<PriceRange> CreateAsync(PriceRange priceRange)
    {
        _dbContext.PriceRanges.Add(priceRange);
        await _dbContext.SaveChangesAsync();
        return priceRange;
    }
}
