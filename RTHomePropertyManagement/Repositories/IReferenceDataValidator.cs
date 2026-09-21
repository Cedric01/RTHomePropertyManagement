namespace RTHomePropertyManagement.Repositories;

// Checks that the foreign keys on an incoming Property payload actually
// point at existing rows, so a bad id turns into a clean 400 instead of an
// unhandled Postgres FK-violation surfacing as a 500.
public interface IReferenceDataValidator
{
    Task<List<string>> GetInvalidPropertyReferencesAsync(
        int? locationId,
        int? listingTypeId,
        int? agentId,
        int? priceRangeId);
}
