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

    // Appends images (in the given order) after any the property already has.
    // Returns null if the property doesn't exist.
    Task<Property?> AddImagesAsync(int id, IReadOnlyList<string> imageUrls);

    // Returns false if the property has no image with that URL.
    Task<bool> RemoveImageAsync(int id, string imageUrl);

    // Number of the owner's properties per status (e.g. ACTIVE, SOLD, RENTED).
    Task<Dictionary<string, int>> GetStatusCountsAsync(string ownerId);
}
