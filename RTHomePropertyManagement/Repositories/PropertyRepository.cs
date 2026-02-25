using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _dbContext;

    public PropertyRepository(RealEstateDbContext dbContext)
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
        return await _dbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PriceRange)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(int id)
    {
        return await _dbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PriceRange)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Property?> UpdateAsync(int id, Property updatedProperty)
    {
        var property = await _dbContext.Properties.FindAsync(id);
        if (property is null) return null;

            property.Title = updatedProperty.Title;
            property.Address = updatedProperty.Address;

            property.LocationId = updatedProperty.LocationId;
            property.ListingTypeId = updatedProperty.ListingTypeId;
            property.AgentId = updatedProperty.AgentId;

            property.IsForRent = updatedProperty.IsForRent;
            property.Price = updatedProperty.Price;
            property.PricePeriod = updatedProperty.PricePeriod;

            property.SquareFeet = updatedProperty.SquareFeet;
            property.Bedrooms = updatedProperty.Bedrooms;
            property.Bathrooms = updatedProperty.Bathrooms;

            property.Status = updatedProperty.Status;

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