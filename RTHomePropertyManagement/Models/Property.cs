using NetTopologySuite.Geometries;
using RTHomePropertyManagement.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;

[Table("properties", Schema = "core")]
public class Property
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // --------------------------------------------------
    // Basic Details
    // --------------------------------------------------

    [Required]
    [MaxLength(255)]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Required]
    [Column("address")]
    public string Address { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    // --------------------------------------------------
    // Foreign Keys
    // --------------------------------------------------

    [Column("location_id")]
    public int? LocationId { get; set; }

    [Column("listing_type_id")]
    public int? ListingTypeId { get; set; }

    [Column("agent_id")]
    public int? AgentId { get; set; }

    [Column("price_range_id")]
    public int? PriceRangeId { get; set; }

    // --------------------------------------------------
    // Listing Details
    // --------------------------------------------------

    [Required]
    [Column("is_for_rent")]
    public bool IsForRent { get; set; }

    [Required]
    [Column("price", TypeName = "numeric(12,2)")]
    public decimal Price { get; set; }

    [Column("price_period")]
    [MaxLength(20)]
    public string? PricePeriod { get; set; } // "month" | "sale"

    // --------------------------------------------------
    // Property Characteristics
    // --------------------------------------------------

    [Column("square_feet")]
    public int? SquareFeet { get; set; }

    [Column("bedrooms")]
    public int? Bedrooms { get; set; }

    [Column("bathrooms")]
    public int? Bathrooms { get; set; }

    // --------------------------------------------------
    // Status & Audit
    // --------------------------------------------------

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "ACTIVE";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --------------------------------------------------
    // Navigation Properties (Optional but Recommended)
    // --------------------------------------------------

    public Location? Location { get; set; }
    public ListingType? ListingType { get; set; }
    public Agent? Agent { get; set; }
    public PriceRange PriceRange { get; set; }
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
}
