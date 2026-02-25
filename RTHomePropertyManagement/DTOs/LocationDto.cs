namespace RTHomePropertyManagement.DTOs;

public class LocationDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string? City { get; set; }
    public string? Country { get; set; }
}