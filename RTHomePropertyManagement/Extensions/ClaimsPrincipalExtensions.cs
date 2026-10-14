using System.Security.Claims;

namespace RTHomePropertyManagement.Extensions;

public static class ClaimsPrincipalExtensions
{
    // The Auth0 user id. JWT "sub" is kept as-is by the default JWT handler, but
    // fall back to the mapped NameIdentifier claim in case mapping is turned on.
    public static string? GetUserId(this ClaimsPrincipal? user)
        => user?.FindFirst("sub")?.Value ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
