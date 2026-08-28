using FrutNatura.Core.Contracts.Auth;

namespace FrutNatura.Core.Abstractions.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
}
