namespace RTHomePropertyManagement.Models;

public record Property
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public double Price { get; init; }
    public int Bedrooms { get; init; }
    public int Bathrooms { get; init; }
}
