using System.ComponentModel.DataAnnotations;

namespace RTHomePropertyManagement.DTOs;

public class EstimateRequestCreateDto
{
    [Required(ErrorMessage = "A valid email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    public string Email { get; set; } = string.Empty;
}
