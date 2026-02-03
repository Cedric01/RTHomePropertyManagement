using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public interface IPriceRangeRepository
{
    Task<List<PriceRange>> GetAllAsync();
    Task<PriceRange> CreateAsync(PriceRange priceRange);
}
