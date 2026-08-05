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
            Furnishing = p.Furnishing,
            YearBuilt = p.YearBuilt,
            Floor = p.Floor,
            Garage = p.Garage,
            CeilingHeight = p.CeilingHeight,
            Renovation = p.Renovation,
            Heating = p.Heating,
            HasIntercom = p.HasIntercom,
            HasAirCondition = p.HasAirCondition,
            WindowType = p.WindowType,
            HasFireplace = p.HasFireplace,
            HasCableTv = p.HasCableTv,
            HasElevator = p.HasElevator,
            HasWifi = p.HasWifi,
            HasVentilation = p.HasVentilation,
            ParkingSpots = p.ParkingSpots,
            GardenSize = p.GardenSize,
            DisabledAccess = p.DisabledAccess,
            HasSwimmingPool = p.HasSwimmingPool,
            HasFence = p.HasFence,
            Security = p.Security,
            IsPetFriendly = p.IsPetFriendly,
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
            Furnishing = dto.Furnishing,
            YearBuilt = dto.YearBuilt,
            Floor = dto.Floor,
            Garage = dto.Garage,
            CeilingHeight = dto.CeilingHeight,
            Renovation = dto.Renovation,
            Heating = dto.Heating,
            HasIntercom = dto.HasIntercom,
            HasAirCondition = dto.HasAirCondition,
            WindowType = dto.WindowType,
            HasFireplace = dto.HasFireplace,
            HasCableTv = dto.HasCableTv,
            HasElevator = dto.HasElevator,
            HasWifi = dto.HasWifi,
            HasVentilation = dto.HasVentilation,
            ParkingSpots = dto.ParkingSpots,
            GardenSize = dto.GardenSize,
            DisabledAccess = dto.DisabledAccess,
            HasSwimmingPool = dto.HasSwimmingPool,
            HasFence = dto.HasFence,
            Security = dto.Security,
            IsPetFriendly = dto.IsPetFriendly,
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
        target.Furnishing = dto.Furnishing;
        target.YearBuilt = dto.YearBuilt;
        target.Floor = dto.Floor;
        target.Garage = dto.Garage;
        target.CeilingHeight = dto.CeilingHeight;
        target.Renovation = dto.Renovation;
        target.Heating = dto.Heating;
        target.HasIntercom = dto.HasIntercom;
        target.HasAirCondition = dto.HasAirCondition;
        target.WindowType = dto.WindowType;
        target.HasFireplace = dto.HasFireplace;
        target.HasCableTv = dto.HasCableTv;
        target.HasElevator = dto.HasElevator;
        target.HasWifi = dto.HasWifi;
        target.HasVentilation = dto.HasVentilation;
        target.ParkingSpots = dto.ParkingSpots;
        target.GardenSize = dto.GardenSize;
        target.DisabledAccess = dto.DisabledAccess;
        target.HasSwimmingPool = dto.HasSwimmingPool;
        target.HasFence = dto.HasFence;
        target.Security = dto.Security;
        target.IsPetFriendly = dto.IsPetFriendly;
        target.Status = dto.Status ?? target.Status;
    }
}
