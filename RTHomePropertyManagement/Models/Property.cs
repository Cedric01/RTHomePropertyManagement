namespace RTHomePropertyManagement.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("properties", Schema = "core")]
public class Property
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool IsForRent { get; set; }

    public decimal Price { get; set; }

    public string? PricePeriod { get; set; }

    public int? SquareFeet { get; set; }
    public int? Bedrooms { get; set; }
    public int? Bathrooms { get; set; }
}
