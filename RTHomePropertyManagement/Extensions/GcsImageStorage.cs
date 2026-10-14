using Google;
using Google.Cloud.Storage.V1;

namespace RTHomePropertyManagement.Extensions;

// Production storage: Google Cloud Storage. Authenticates with the Cloud Run
// service account (Application Default Credentials). The bucket must allow
// public reads so the stored URL works in <img> tags, and the service account
// needs the "Storage Object Admin" role on it.
public class GcsImageStorage : IImageStorage
{
    private readonly StorageClient _client;
    private readonly string _bucket;
    private readonly string _urlPrefix;

    public GcsImageStorage(IConfiguration config)
    {
        _bucket = config["ImageStorage:GcsBucket"]
            ?? throw new InvalidOperationException(
                "ImageStorage:GcsBucket must be set when ImageStorage:Provider is Gcs.");
        _urlPrefix = $"https://storage.googleapis.com/{_bucket}/";
        _client = StorageClient.Create();
    }

    public async Task<string> SaveAsync(Stream content, string extension, string contentType, CancellationToken ct = default)
    {
        var objectName = $"properties/{Guid.NewGuid():N}{extension}";
        await _client.UploadObjectAsync(_bucket, objectName, contentType, content, cancellationToken: ct);
        return _urlPrefix + objectName;
    }

    public async Task DeleteAsync(string url, CancellationToken ct = default)
    {
        if (!url.StartsWith(_urlPrefix, StringComparison.Ordinal)) return;

        var objectName = Uri.UnescapeDataString(url[_urlPrefix.Length..]);
        try
        {
            await _client.DeleteObjectAsync(_bucket, objectName, cancellationToken: ct);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Already gone.
        }
    }
}
