namespace RTHomePropertyManagement.DTOs;

public class PriceRangeDto
{
    public int Id { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string DisplayLabel { get; set; } = string.Empty;
}