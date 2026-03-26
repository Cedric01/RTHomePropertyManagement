using System;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;

namespace RTHomePropertyManagement.Extensions;

public static class MappingExtensions
{
    public static PropertyDto ToDto(this Property p)
    {
        if (p is null) throw new ArgumentNullException(nameof(p));

        return new PropertyDto
        {
            Id = p.Id,
            Title = p.Title,
            Address = p.Address,
            Description = p.Description,
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
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            Location = p.Location is not null ? new LocationDto
            {
                Id = p.Location.Id,
                DisplayName = p.Location.DisplayName,
                City = p.Location.City,
                Country = p.Location.Country
            } : null,
            PriceRange = p.PriceRange is not null ? new PriceRangeDto
            {
                Id = p.PriceRange.Id,
                MinPrice = p.PriceRange.MinPrice,
                MaxPrice = p.PriceRange.MaxPrice,
                DisplayLabel = p.PriceRange.DisplayLabel
            } : null,
            ImageUrls = p.Images
                .OrderBy(i => i.SortOrder)
                .Select(i => i.ImageUrl)
                .ToList()
        };
    }

    public static Property ToModel(this PropertyCreateDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        return new Property
        {
            Title = dto.Title,
            Address = dto.Address,
            Description = dto.Description,
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
            Status = dto.Status ?? "ACTIVE"
        };
    }

    public static void ApplyUpdate(this Property target, PropertyUpdateDto dto)
    {
        if (target is null) throw new ArgumentNullException(nameof(target));
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        target.Title = dto.Title;
        target.Address = dto.Address;
        target.Description = dto.Description;
        target.LocationId = dto.LocationId;
        target.ListingTypeId = dto.ListingTypeId;
        target.AgentId = dto.AgentId;
        target.PriceRangeId = dto.PriceRangeId;
        target.IsForRent = dto.IsForRent;
        target.Price = dto.Price;
        target.PricePeriod = dto.PricePeriod;
        target.SquareFeet = dto.SquareFeet;
        target.Bedrooms = dto.Bedrooms;
        target.Bathrooms = dto.Bathrooms;
        target.Status = dto.Status ?? target.Status;
    }
}