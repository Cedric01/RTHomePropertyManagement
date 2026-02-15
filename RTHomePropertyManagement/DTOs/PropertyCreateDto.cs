namespace RTHomePropertyManagement.DTOs;

public class PropertyCreateDto
{
    // Basic details
    public string Title { get; set; } = null!;
    public string Address { get; set; } = null!;

    // Foreign keys
    public int? LocationId { get; set; }
    public int? ListingTypeId { get; set; }
    public int? AgentId { get; set; }
    public int? PriceRangeId { get; set; }

    // Listing details
    public bool IsForRent { get; set; }
    public decimal Price { get; set; }
    public string? PricePeriod { get; set; }

    // Characteristics
    public int? SquareFeet { get; set; }
    public int? Bedrooms { get; set; }
    public int? Bathrooms { get; set; }

    // Optional
    public string? Status { get; set; }
}