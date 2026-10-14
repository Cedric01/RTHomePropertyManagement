namespace RTHomePropertyManagement.Extensions;

// Where uploaded property photos live. The database only stores the URL that
// SaveAsync returns.
public interface IImageStorage
{
    // extension includes the leading dot, e.g. ".jpg".
    Task<string> SaveAsync(Stream content, string extension, string contentType, CancellationToken ct = default);

    // Best effort: does nothing if the URL isn't one this storage produced.
    Task DeleteAsync(string url, CancellationToken ct = default);
}
