using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;

[Table("agents", Schema = "core")]
public class Agent
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [MaxLength(100)]
    [Column("designation")]
    public string? Designation { get; set; }

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [Column("profile_link")]
    public string? ProfileLink { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [MaxLength(30)]
    [Column("phone")]
    public string? Phone { get; set; }

    [MaxLength(150)]
    [Column("location")]
    public string? Location { get; set; }

    // --------------------------------------------------
    // Navigation Properties
    // --------------------------------------------------

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
