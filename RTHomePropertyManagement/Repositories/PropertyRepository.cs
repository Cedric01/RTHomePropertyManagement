using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

public class PropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _dbContext;

    public PropertyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Property> CreateAsync(Property property)
    {
        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();
        return property;
    }

    public async Task<List<Property>> GetAllAsync()
    {
        return await _dbContext.Properties.ToListAsync();
    }

    public async Task<List<PropertyListing>> GetAllListingPropertiesAsync()
    {
        return await _dbContext.PropertiesListing.ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(int id)
    {
        return await _dbContext.Properties.FindAsync(id);
    }

    public async Task<Property?> UpdateAsync(int id, Property updatedProperty)
    {
        var property = await _dbContext.Properties.FindAsync(id);
        if (property is null) return null;

        property = property with
        {
            Name = updatedProperty.Name,
            City = updatedProperty.City,
            State = updatedProperty.State,
            Photo = updatedProperty.Photo,
            AvailableUnits = updatedProperty.AvailableUnits,
            Wifi = updatedProperty.Wifi,
            Laundry = updatedProperty.Laundry
        };

        _dbContext.Entry(property).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return property;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var property = await _dbContext.Properties.FindAsync(id);
        if (property is null) return false;

        _dbContext.Properties.Remove(property);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}