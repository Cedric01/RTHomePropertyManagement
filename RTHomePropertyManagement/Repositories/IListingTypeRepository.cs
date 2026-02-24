using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public interface IListingTypeRepository
{
    Task<List<ListingType>> GetAllAsync();
    Task<ListingType?> GetByIdAsync(int id);
}
