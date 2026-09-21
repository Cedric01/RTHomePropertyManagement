using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertyManagementTests;

// Always reports every reference as valid - the default for tests that don't
// care about FK validation. Tests that do care construct their own
// Mock<IReferenceDataValidator> and pass it in explicitly instead.
internal class AlwaysValidReferenceDataValidator : IReferenceDataValidator
{
    public Task<List<string>> GetInvalidPropertyReferencesAsync(
        int? locationId, int? listingTypeId, int? agentId, int? priceRangeId)
        => Task.FromResult(new List<string>());
}

public static class PropertyEndpointTestHelpers
{
    public static Task<IResult> InvokeCreateProperty(
        IPropertyRepository repo,
        Property p,
        IReferenceDataValidator? referenceValidator = null)
        => PropertyEndpoints.CreateProperty(
            repo,
            referenceValidator ?? new AlwaysValidReferenceDataValidator(),
            NullLogger<EndpointLogCategory>.Instance,
            new PropertyCreateDto
            {
                Title = p.Title,
                Address = p.Address,
                LocationId = p.LocationId,
                ListingTypeId = p.ListingTypeId,
                AgentId = p.AgentId,
                PriceRangeId = p.PriceRangeId,
                IsForRent = p.IsForRent,
                Price = p.Price,
                PricePeriod = p.PricePeriod,
                SquareFeet = p.SquareFeet,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                Status = p.Status
            });

    public static Task<IResult> InvokeListProperties(IPropertyRepository repo)
        => PropertyEndpoints.ListProperties(
            repo,
            new DefaultHttpContext().Response,
            locationId: null,
            listingTypeId: null,
            isForRent: null,
            minPrice: null,
            maxPrice: null,
            minBedrooms: null,
            page: null,
            pageSize: null);

    public static Task<IResult> InvokeUpdateProperty(
        IPropertyRepository repo,
        int id,
        Property p,
        IReferenceDataValidator? referenceValidator = null)
        => PropertyEndpoints.UpdateProperty(
            repo,
            referenceValidator ?? new AlwaysValidReferenceDataValidator(),
            NullLogger<EndpointLogCategory>.Instance,
            id,
            new PropertyUpdateDto
            {
                Title = p.Title,
                Address = p.Address,
                LocationId = p.LocationId,
                ListingTypeId = p.ListingTypeId,
                AgentId = p.AgentId,
                PriceRangeId = p.PriceRangeId,
                IsForRent = p.IsForRent,
                Price = p.Price,
                PricePeriod = p.PricePeriod,
                SquareFeet = p.SquareFeet,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                Status = p.Status
            });

    public static Task<IResult> InvokeDeleteProperty(IPropertyRepository repo, int id)
        => PropertyEndpoints.DeleteProperty(repo, NullLogger<EndpointLogCategory>.Instance, id);
}
