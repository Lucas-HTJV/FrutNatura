
namespace FrutNatura.Core.Contracts.Auth;
public sealed class RegisterRequest
{
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string HashPassword { get; init; } = string.Empty;
}
