
namespace FrutNatura.Core.Contracts.Auth;
public sealed record LoginResponse(string AccessToken,
        bool Success = true,
        string? Error = null,
        string? RefreshToken = null,
        Guid UsuarioId = default,
        string? Name = null,
        string? Role = null);
