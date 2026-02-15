using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;

namespace RTHomePropertyManagementTests;

public static class PropertyEndpointTestHelpers
{
    public static Task<IResult> InvokeCreateProperty(IPropertyRepository repo, Property p)
        => PropertyEndpoints.CreateProperty(repo, new PropertyCreateDto
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
        => PropertyEndpoints.ListProperties(repo);

    public static Task<IResult> InvokeUpdateProperty(IPropertyRepository repo, int id, Property p)
        => PropertyEndpoints.UpdateProperty(repo, id, new PropertyUpdateDto
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
        => PropertyEndpoints.DeleteProperty(repo, id);
}