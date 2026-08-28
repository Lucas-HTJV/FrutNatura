using System.Security.Claims;

namespace FrutNatura.Infra.Security.Utils;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(sub, out var id) ? id : null;
    }

    public static string? GetEmail(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue("email");

    public static bool HasAnyRole(this ClaimsPrincipal user, params string[] roles)
    {
        if (roles is null || roles.Length == 0) return false;
        return roles.Any(r => user.IsInRole(r));
    }
}
