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
    // Property Details (accordion)
    // --------------------------------------------------

    [Column("furnishing")]
    [MaxLength(50)]
    public string? Furnishing { get; set; } // e.g. "Furnished", "Semi furnished", "Unfurnished"

    [Column("year_built")]
    public int? YearBuilt { get; set; }

    [Column("floor")]
    [MaxLength(50)]
    public string? Floor { get; set; } // e.g. "Ground", "1st"

    [Column("garage")]
    public int? Garage { get; set; }

    [Column("ceiling_height")]
    [MaxLength(20)]
    public string? CeilingHeight { get; set; } // e.g. "3.2m"

    [Column("renovation")]
    [MaxLength(100)]
    public string? Renovation { get; set; }

    // --------------------------------------------------
    // Utility Details (accordion)
    // --------------------------------------------------

    [Column("heating")]
    [MaxLength(50)]
    public string? Heating { get; set; } // e.g. "Natural gas"

    [Column("has_intercom")]
    public bool? HasIntercom { get; set; }

    [Column("has_air_condition")]
    public bool? HasAirCondition { get; set; }

    [Column("window_type")]
    [MaxLength(50)]
    public string? WindowType { get; set; } // e.g. "Aluminum frame"

    [Column("has_fireplace")]
    public bool? HasFireplace { get; set; }

    [Column("has_cable_tv")]
    public bool? HasCableTv { get; set; }

    [Column("has_elevator")]
    public bool? HasElevator { get; set; }

    [Column("has_wifi")]
    public bool? HasWifi { get; set; }

    [Column("has_ventilation")]
    public bool? HasVentilation { get; set; }

    // --------------------------------------------------
    // Outdoor Features (accordion)
    // --------------------------------------------------

    [Column("parking_spots")]
    public int? ParkingSpots { get; set; }

    [Column("garden_size")]
    [MaxLength(50)]
    public string? GardenSize { get; set; } // e.g. "30m2"

    [Column("disabled_access")]
    [MaxLength(100)]
    public string? DisabledAccess { get; set; } // e.g. "Ramp", "Yes"

    [Column("has_swimming_pool")]
    public bool? HasSwimmingPool { get; set; }

    [Column("has_fence")]
    public bool? HasFence { get; set; }

    [Column("security")]
    [MaxLength(100)]
    public string? Security { get; set; } // e.g. "3 Cameras"

    [Column("is_pet_friendly")]
    public bool? IsPetFriendly { get; set; }

    // --------------------------------------------------
    // Status & Audit
    // --------------------------------------------------

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "ACTIVE";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --------------------------------------------------
    // Concurrency
    // --------------------------------------------------

    // Maps to Postgres's built-in xmin system column via Npgsql's EF Core
    // convention for [Timestamp] + uint. Gives optimistic concurrency (a 409
    // on conflicting concurrent edits, see PropertyRepository.UpdateAsync)
    // with no schema migration needed, since xmin already exists on every
    // row.
    [Timestamp]
    public uint Version { get; set; }

    // --------------------------------------------------
    // Navigation Properties
    // --------------------------------------------------

    public Location? Location { get; set; }
    public ListingType? ListingType { get; set; }
    public Agent? Agent { get; set; }
    public PriceRange PriceRange { get; set; }
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
}
