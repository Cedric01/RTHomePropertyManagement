namespace RTHomePropertyManagement.Models;

public record PropertyListing
{
    public string Id { get; init; } = string.Empty;
    public string Tag { get; init; } = string.Empty;
    public string[] Images { get; init; } = Array.Empty<string>();
    public string Title { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public Icons[] Features { get; init; } = Array.Empty<Icons>();
    public string Price { get; init; } = string.Empty;
    public bool Rent
    {
        get; init;
    }

}
