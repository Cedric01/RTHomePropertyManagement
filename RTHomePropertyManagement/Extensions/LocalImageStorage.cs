namespace RTHomePropertyManagement.Extensions;

// Development storage: writes files under an "uploads" folder next to the app
// and serves them from /uploads (see ImageStorageExtensions.UseLocalImageFiles).
// Not for Cloud Run - its disk is wiped whenever an instance restarts.
public class LocalImageStorage : IImageStorage
{
    public const string RequestPath = "/uploads";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalImageStorage(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        RootPath = Path.Combine(environment.ContentRootPath, "uploads");
        Directory.CreateDirectory(RootPath);
        _httpContextAccessor = httpContextAccessor;
    }

    public string RootPath { get; }

    public async Task<string> SaveAsync(Stream content, string extension, string contentType, CancellationToken ct = default)
    {
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(RootPath, fileName);

        await using (var file = File.Create(path))
        {
            await content.CopyToAsync(file, ct);
        }

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = request is null ? string.Empty : $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}{RequestPath}/{fileName}";
    }

    public Task DeleteAsync(string url, CancellationToken ct = default)
    {
        if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)) return Task.CompletedTask;

        var urlPath = uri.IsAbsoluteUri ? uri.AbsolutePath : url;
        if (!urlPath.StartsWith(RequestPath + "/", StringComparison.Ordinal)) return Task.CompletedTask;

        // GetFileName drops any directory parts, so this can't escape RootPath.
        var fileName = Path.GetFileName(urlPath);
        var path = Path.Combine(RootPath, fileName);
        if (File.Exists(path)) File.Delete(path);

        return Task.CompletedTask;
    }
}
