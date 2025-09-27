using RTHomePropertyManagement.Models;

public interface IPropertyRepository
{
    Task<Property> CreateAsync(Property property);
    Task<List<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(int id);
    Task<Property?> UpdateAsync(int id, Property updatedProperty);
    Task<bool> DeleteAsync(int id);
}