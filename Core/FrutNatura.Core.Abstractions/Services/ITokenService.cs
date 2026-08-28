namespace FrutNatura.Core.Abstractions.Services;

public interface ITokenService
{
    string CreateAccessToken(Guid userId, string email, IEnumerable<string> roles, CancellationToken ct = default);
}
