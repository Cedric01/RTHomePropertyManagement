using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTHomePropertyManagement.Models;

[Table("price_ranges", Schema = "core")]
public class PriceRange : IValidatableObject
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "minPrice cannot be negative.")]
    [Column("min_price")]
    public decimal MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "maxPrice cannot be negative.")]
    [Column("max_price")]
    public decimal MaxPrice { get; set; }

    [Required(ErrorMessage = "displayLabel is required.")]
    [Column("display_label")]
    public string DisplayLabel { get; set; } = string.Empty;

    public ICollection<Property> Properties { get; set; } = new List<Property>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MaxPrice < MinPrice)
        {
            yield return new ValidationResult(
                "maxPrice must be greater than or equal to minPrice.",
                new[] { nameof(MaxPrice) });
        }
    }
}
