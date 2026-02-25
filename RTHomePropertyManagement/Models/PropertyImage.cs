using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;

[Table("property_images", Schema = "core")]
public class PropertyImage
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("property_id")]
    public int PropertyId { get; set; }

    [Required]
    [Column("image_url")]
    public string ImageUrl { get; set; } = null!;

    [Column("sort_order")]
    public int SortOrder { get; set; } = 0;

    public Property Property { get; set; } = null!;
}