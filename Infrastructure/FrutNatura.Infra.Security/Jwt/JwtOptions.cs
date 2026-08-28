namespace FrutNatura.Infra.Security.Options;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
    public int AccessTokenMinutes { get; set; } = 8;
    public int RefeshTokenDays { get; set; } = 7;
}
