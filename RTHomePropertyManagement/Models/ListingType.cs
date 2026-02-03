using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;

[Table("listing_types", Schema = "core")]
public class ListingType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("label")]
    public string Label { get; set; } = null!;

    // --------------------------------------------------
    // Navigation Properties
    // --------------------------------------------------

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
