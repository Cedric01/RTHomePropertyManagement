namespace RTHomePropertyManagement.Models;

public record Property
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string? Photo { get; init; }
    public int AvailableUnits { get; init; }
    public bool Wifi { get; init; }
    public bool Laundry { get; init; }
}
