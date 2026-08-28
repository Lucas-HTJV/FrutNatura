
namespace FrutNatura.Core.Contracts.Auth;
public sealed record RefreshTokenResponse(string AccessToken, bool Success = true, string? Error = null);
