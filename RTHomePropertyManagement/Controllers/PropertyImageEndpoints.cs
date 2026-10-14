using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertyManagement.Controllers;

public static class PropertyImageEndpoints
{
    public const long MaxFileBytes = 5 * 1024 * 1024;
    public const int MaxFilesPerRequest = 5;
    public const int MaxImagesPerProperty = 20;

    private static readonly Dictionary<string, string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["image/jpeg"] = ".jpg",
        ["image/png"] = ".png",
        ["image/webp"] = ".webp"
    };

    public static IEndpointRouteBuilder MapPropertyImageEndpoints(this IEndpointRouteBuilder app)
    {
        // The API authenticates with bearer tokens, not cookies, so form
        // antiforgery doesn't apply (and no antiforgery middleware is configured).
        app.MapPost("/properties/{id:int}/images", UploadImages)
            .RequireAuthorization("Agent")
            .AddEndpointFilter<PropertyOwnerFilter>()
            .DisableAntiforgery();

        app.MapDelete("/properties/{id:int}/images", DeleteImage)
            .RequireAuthorization("Agent")
            .AddEndpointFilter<PropertyOwnerFilter>();

        return app;
    }

    public static async Task<IResult> UploadImages(
        int id,
        IFormFileCollection files,
        IPropertyRepository repository,
        IImageStorage storage,
        ILogger<EndpointLogCategory> logger,
        CancellationToken ct)
    {
        if (files.Count == 0)
            return FileProblem("Choose at least one photo to upload.");
        if (files.Count > MaxFilesPerRequest)
            return FileProblem($"Upload at most {MaxFilesPerRequest} photos at a time.");

        foreach (var file in files)
        {
            if (file.Length == 0)
                return FileProblem($"{file.FileName} is empty.");
            if (file.Length > MaxFileBytes)
                return FileProblem($"{file.FileName} is larger than {MaxFileBytes / (1024 * 1024)} MB.");
            if (!AllowedTypes.TryGetValue(file.ContentType, out var ext))
                return FileProblem($"{file.FileName} must be a JPEG, PNG or WebP image.");
            if (!await HasImageSignatureAsync(file, ext, ct))
                return FileProblem($"{file.FileName} is not a valid image.");
        }

        var property = await repository.GetByIdAsync(id);
        if (property is null) return Results.NotFound();
        if (property.Images.Count + files.Count > MaxImagesPerProperty)
            return FileProblem($"A property can have at most {MaxImagesPerProperty} photos.");

        var urls = new List<string>();
        try
        {
            foreach (var file in files)
            {
                await using var stream = file.OpenReadStream();
                urls.Add(await storage.SaveAsync(stream, AllowedTypes[file.ContentType], file.ContentType, ct));
            }

            var updated = await repository.AddImagesAsync(id, urls);
            if (updated is null) return Results.NotFound();

            logger.LogInformation("Added {Count} image(s) to property {PropertyId}", urls.Count, id);
            return Results.Ok(updated.ToDto());
        }
        catch
        {
            // Don't leave orphaned files behind if saving or the DB write failed.
            foreach (var url in urls)
            {
                try { await storage.DeleteAsync(url); } catch { /* best effort */ }
            }
            throw;
        }
    }

    public static async Task<IResult> DeleteImage(
        int id,
        string url,
        IPropertyRepository repository,
        IImageStorage storage,
        ILogger<EndpointLogCategory> logger,
        CancellationToken ct)
    {
        var removed = await repository.RemoveImageAsync(id, url);
        if (!removed) return Results.NotFound();

        try
        {
            await storage.DeleteAsync(url, ct);
        }
        catch (Exception ex)
        {
            // The row is gone, so the photo no longer shows; an orphaned file is harmless.
            logger.LogWarning(ex, "Could not delete stored image file for property {PropertyId}", id);
        }

        logger.LogInformation("Removed an image from property {PropertyId}", id);
        return Results.NoContent();
    }

    private static IResult FileProblem(string message) =>
        Results.ValidationProblem(new Dictionary<string, string[]> { ["files"] = new[] { message } });

    // Checks the first bytes match the claimed type, so a renamed .exe or HTML
    // file can't be uploaded as a photo.
    private static async Task<bool> HasImageSignatureAsync(IFormFile file, string extension, CancellationToken ct)
    {
        var header = new byte[12];
        await using var stream = file.OpenReadStream();
        var read = 0;
        while (read < header.Length)
        {
            var n = await stream.ReadAsync(header.AsMemory(read), ct);
            if (n == 0) break;
            read += n;
        }
        if (read < header.Length) return false;

        return extension switch
        {
            ".jpg" => header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
            ".png" => header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47
                      && header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A,
            ".webp" => header[0] == (byte)'R' && header[1] == (byte)'I' && header[2] == (byte)'F' && header[3] == (byte)'F'
                       && header[8] == (byte)'W' && header[9] == (byte)'E' && header[10] == (byte)'B' && header[11] == (byte)'P',
            _ => false
        };
    }
}
