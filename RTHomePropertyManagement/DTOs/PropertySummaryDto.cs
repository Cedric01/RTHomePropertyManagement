namespace RTHomePropertyManagement.DTOs;

// Counts shown on the dashboard home for the logged-in user's own properties.
public class PropertySummaryDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Sold { get; set; }
    public int Rented { get; set; }
}
