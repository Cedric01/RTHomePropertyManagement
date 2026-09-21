using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _dbContext;
    private readonly ILogger<PropertyRepository> _logger;

    public PropertyRepository(RealEstateDbContext dbContext, ILogger<PropertyRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Property> CreateAsync(Property property)
    {
        _dbContext.Properties.Add(property);
        await _dbContext.SaveChangesAsync();
        return property;
    }

    public async Task<(List<Property> Items, int TotalCount)> GetAllAsync(PropertyFilter? filter = null)
    {
        var query = _dbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PriceRange)
            .Include(p => p.Images)
            .AsQueryable();

        if (filter is not null)
        {
            if (filter.LocationId is int locationId)
                query = query.Where(p => p.LocationId == locationId);

            if (filter.ListingTypeId is int listingTypeId)
                query = query.Where(p => p.ListingTypeId == listingTypeId);

            if (filter.IsForRent is bool isForRent)
                query = query.Where(p => p.IsForRent == isForRent);

            if (filter.MinPrice is decimal minPrice)
                query = query.Where(p => p.Price >= minPrice);

            if (filter.MaxPrice is decimal maxPrice)
                query = query.Where(p => p.Price <= maxPrice);

            if (filter.MinBedrooms is int minBedrooms)
                query = query.Where(p => p.Bedrooms >= minBedrooms);
        }

        query = query.OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync();

        // Paging only kicks in when both page and a positive pageSize are
        // supplied - omit them and every matching row comes back, same as
        // the old behavior, so existing callers aren't affected.
        if (filter?.Page is int page && filter.PageSize is int pageSize && pageSize > 0)
        {
            var safePage = Math.Max(page, 1);
            var safePageSize = Math.Min(pageSize, 100); // hard cap so a bad client can't pull the whole table in one page
            query = query.Skip((safePage - 1) * safePageSize).Take(safePageSize);
        }

        var items = await query.ToListAsync();
        return (items, totalCount);
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

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating property {PropertyId}", id);
            throw new ConcurrencyConflictException(
                $"Property {id} was modified by another request. Reload and try again.");
        }

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
