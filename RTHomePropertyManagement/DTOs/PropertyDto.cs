using System;

namespace RTHomePropertyManagement.DTOs;

public class PropertyDto
{
    public int Id { get; set; }

    // Basic details
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Description { get; set; }

    // Foreign keys
    public int? LocationId { get; set; }
    public int? ListingTypeId { get; set; }
    public int? AgentId { get; set; }
    public int? PriceRangeId { get; set; }

    // Navigation dto references
    public LocationDto? Location { get; set; }
    public PriceRangeDto? PriceRange { get; set; }

    // Listing details
    public bool IsForRent { get; set; }
    public decimal Price { get; set; }
    public string? PricePeriod { get; set; }

    // Characteristics
    public int? SquareFeet { get; set; }
    public int? Bedrooms { get; set; }
    public int? Bathrooms { get; set; }

    // Property Details
    public string? Furnishing { get; set; }
    public int? YearBuilt { get; set; }
    public string? Floor { get; set; }
    public int? Garage { get; set; }
    public string? CeilingHeight { get; set; }
    public string? Renovation { get; set; }

    // Utility Details
    public string? Heating { get; set; }
    public bool? HasIntercom { get; set; }
    public bool? HasAirCondition { get; set; }
    public string? WindowType { get; set; }
    public bool? HasFireplace { get; set; }
    public bool? HasCableTv { get; set; }
    public bool? HasElevator { get; set; }
    public bool? HasWifi { get; set; }
    public bool? HasVentilation { get; set; }

    // Outdoor Features
    public int? ParkingSpots { get; set; }
    public string? GardenSize { get; set; }
    public string? DisabledAccess { get; set; }
    public bool? HasSwimmingPool { get; set; }
    public bool? HasFence { get; set; }
    public string? Security { get; set; }
    public bool? IsPetFriendly { get; set; }

    // Status & audit
    public string Status { get; set; } = "ACTIVE";
    public DateTime CreatedAt { get; set; }

    // Images
    public List<string> ImageUrls { get; set; } = new();
}
