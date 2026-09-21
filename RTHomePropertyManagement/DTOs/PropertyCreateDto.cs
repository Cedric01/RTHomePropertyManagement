using System.ComponentModel.DataAnnotations;

namespace RTHomePropertyManagement.DTOs;

public class PropertyCreateDto
{
    // Basic details
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    // Foreign keys - existence is checked separately against the database
    // (see IReferenceDataValidator), these just reject obviously-bogus ids.
    [Range(1, int.MaxValue, ErrorMessage = "locationId must be a positive id.")]
    public int? LocationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "listingTypeId must be a positive id.")]
    public int? ListingTypeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "agentId must be a positive id.")]
    public int? AgentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "priceRangeId must be a positive id.")]
    public int? PriceRangeId { get; set; }

    // Listing details
    public bool IsForRent { get; set; }

    [Range(typeof(decimal), "0.01", "999999999999.99", ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [RegularExpression("^(month|sale)$", ErrorMessage = "pricePeriod must be either 'month' or 'sale'.")]
    public string? PricePeriod { get; set; }

    // Characteristics
    [Range(0, int.MaxValue)]
    public int? SquareFeet { get; set; }

    [Range(0, int.MaxValue)]
    public int? Bedrooms { get; set; }

    [Range(0, int.MaxValue)]
    public int? Bathrooms { get; set; }

    // Property Details
    [MaxLength(50)]
    public string? Furnishing { get; set; }

    [Range(1600, 2100)]
    public int? YearBuilt { get; set; }

    [MaxLength(50)]
    public string? Floor { get; set; }

    [Range(0, int.MaxValue)]
    public int? Garage { get; set; }

    [MaxLength(20)]
    public string? CeilingHeight { get; set; }

    [MaxLength(100)]
    public string? Renovation { get; set; }

    // Utility Details
    [MaxLength(50)]
    public string? Heating { get; set; }

    public bool? HasIntercom { get; set; }
    public bool? HasAirCondition { get; set; }

    [MaxLength(50)]
    public string? WindowType { get; set; }

    public bool? HasFireplace { get; set; }
    public bool? HasCableTv { get; set; }
    public bool? HasElevator { get; set; }
    public bool? HasWifi { get; set; }
    public bool? HasVentilation { get; set; }

    // Outdoor Features
    [Range(0, int.MaxValue)]
    public int? ParkingSpots { get; set; }

    [MaxLength(50)]
    public string? GardenSize { get; set; }

    [MaxLength(100)]
    public string? DisabledAccess { get; set; }

    public bool? HasSwimmingPool { get; set; }
    public bool? HasFence { get; set; }

    [MaxLength(100)]
    public string? Security { get; set; }

    public bool? IsPetFriendly { get; set; }

    // Optional
    [MaxLength(20)]
    public string? Status { get; set; }
}
