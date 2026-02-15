using System;

namespace RTHomePropertyManagement.DTOs;

public class PropertyDto
{
    public int Id { get; set; }

    // Basic details
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;

    // Foreign keys
    public int? LocationId { get; set; }
    public int? ListingTypeId { get; set; }
    public int? AgentId { get; set; }
    public int? PriceRangeId { get; set; }

    // Navigation dto references (optional)
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

    // Status & audit
    public string Status { get; set; } = "ACTIVE";
    public DateTime CreatedAt { get; set; }
}