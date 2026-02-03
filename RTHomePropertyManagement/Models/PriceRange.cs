
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;


[Table("price_ranges", Schema = "core")]
public class PriceRange
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("min_price")]
    public decimal MinPrice { get; set; }

    [Column("max_price")]
    public decimal MaxPrice { get; set; }

    [Column("display_label")]
    public string DisplayLabel { get; set; } = string.Empty;

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}


