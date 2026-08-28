
namespace FrutNatura.Core.Contracts.Auth;
public sealed class LoginRequest
{
    public string Email { get; init; } = string.Empty;
    public string HashPassword { get; init; } = string.Empty;
}
