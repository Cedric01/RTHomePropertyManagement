using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

public interface IPropertyRepository
{
    Task<Property> CreateAsync(Property property);

    // filter is optional - GetAllAsync(null) (or the default) behaves exactly
    // like the old unfiltered/unpaginated GetAllAsync() did.
    Task<(List<Property> Items, int TotalCount)> GetAllAsync(PropertyFilter? filter = null);
    Task<Property?> GetByIdAsync(int id);

    // Throws ConcurrencyConflictException if the row was modified by another
    // request between the caller's read and this write.
    Task<Property?> UpdateAsync(int id, Property updatedProperty);
    Task<bool> DeleteAsync(int id);
}
