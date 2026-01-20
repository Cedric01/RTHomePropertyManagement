using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;

[Table("locations", Schema = "core")]
public class Location
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(100)]
    [Column("city")]
    public string? City { get; set; }

    [MaxLength(100)]
    [Column("country")]
    public string? Country { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("display_name")]
    public string DisplayName { get; set; } = null!;

    // --------------------------------------------------
    // Navigation Properties
    // --------------------------------------------------

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
