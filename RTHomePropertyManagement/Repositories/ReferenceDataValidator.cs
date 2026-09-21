using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public class ReferenceDataValidator : IReferenceDataValidator
{
    private readonly RealEstateDbContext _dbContext;

    public ReferenceDataValidator(RealEstateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<string>> GetInvalidPropertyReferencesAsync(
        int? locationId,
        int? listingTypeId,
        int? agentId,
        int? priceRangeId)
    {
        var invalid = new List<string>();

        if (locationId is int locId && !await _dbContext.Locations.AnyAsync(l => l.Id == locId))
            invalid.Add("locationId");

        if (listingTypeId is int ltId && !await _dbContext.ListingTypes.AnyAsync(lt => lt.Id == ltId))
            invalid.Add("listingTypeId");

        if (agentId is int agId && !await _dbContext.Agents.AnyAsync(a => a.Id == agId))
            invalid.Add("agentId");

        if (priceRangeId is int prId && !await _dbContext.PriceRanges.AnyAsync(p => p.Id == prId))
            invalid.Add("priceRangeId");

        return invalid;
    }
}
