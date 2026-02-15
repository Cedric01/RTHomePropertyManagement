// Plan (pseudocode):
// 1. Create an extensions class in namespace RTHomePropertyManagement.Extensions.
// 2. Add a public static extension method `ToModel` that accepts `PropertyCreateDto`.
// 3. Validate input (throw ArgumentNullException if null).
// 4. Construct and return a new `Property` instance, copying fields from the DTO.
// 5. Keep mapping names aligned with DTO properties: Title, Address, LocationId, ListingTypeId,
//    AgentId, PriceRangeId, IsForRent, Price, PricePeriod, SquareFeet, Bedrooms, Bathrooms, Status.
// 6. Place using directives for required namespaces.
// 7. This resolves CS1061 by providing the missing extension method used in `PropertyEndpoints.CreateProperty`.
//
// Note: This implementation assumes the `Property` model exposes writable properties
// with the same names/types as the DTO. If the model's shape differs, adjust the mappings
// to match the `Property` class members.

using System;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;

namespace RTHomePropertyManagement.Extensions
{
    public static class PropertyDtoExtensions
    {
        public static Property ToModel(this PropertyCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new Property
            {
                Title = dto.Title,
                Address = dto.Address,
                LocationId = dto.LocationId,
                ListingTypeId = dto.ListingTypeId,
                AgentId = dto.AgentId,
                PriceRangeId = dto.PriceRangeId,
                IsForRent = dto.IsForRent,
                Price = dto.Price,
                PricePeriod = dto.PricePeriod,
                SquareFeet = dto.SquareFeet,
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                Status = dto.Status
            };
        }
    }
}