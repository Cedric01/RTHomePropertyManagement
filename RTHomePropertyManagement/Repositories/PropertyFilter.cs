namespace RTHomePropertyManagement.Repositories;

// Optional filter/pagination options for listing properties. Every field is
// optional and additive - a request with none of these set behaves exactly
// like the old unfiltered, unpaginated GetAllAsync() did, so existing
// frontend calls keep working unchanged.
public class PropertyFilter
{
    public int? LocationId { get; set; }
    public int? ListingTypeId { get; set; }
    public bool? IsForRent { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinBedrooms { get; set; }

    // Paging only kicks in when both are supplied and PageSize is positive.
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
