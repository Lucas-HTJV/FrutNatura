using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Security.Roles;
using FrutNatura.Infra.Security.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrutNatura.Infra.Security.Jwt;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtOptions _opt;
    public JwtTokenService(IOptions<JwtOptions> opt) => _opt = opt.Value;

    public string CreateAccessToken(Guid userId, string email, IEnumerable<string> roles, CancellationToken ct = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email ?? string.Empty),
            new(ClaimTypes.Name, email ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
            claims.Add(new Claim(ClaimTypes.Name, email));
        }
        var r = roles?.ToArray() ?? Array.Empty<string>();
        if (r.Length == 0) r = new[] { SystemRoles.Cliente };
        claims.AddRange(r.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Math.Max(1, _opt.AccessTokenMinutes)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
