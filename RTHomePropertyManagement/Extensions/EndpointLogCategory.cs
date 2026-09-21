namespace RTHomePropertyManagement.Extensions;

// Shared ILogger<T> category for the static minimal-API endpoint classes
// (PropertyEndpoints, EstimateRequestEndpoints, PriceRangeEndpoints, ...),
// which can't be used directly as a generic type argument since they're
// static classes.
public sealed class EndpointLogCategory
{
}
