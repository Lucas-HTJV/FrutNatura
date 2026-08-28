namespace FrutNatura.Core.Security.Claims;

/// <summary>
/// Tipos de claim customizados usados no token/JWT.
/// </summary>
public static class CustomClaimTypes
{
    // Preferimos usar os padrões (sub/email/name/role), mas ficam exemplos:
    public const string Tenant = "tn";      // multi-empresa, se um dia precisar
    public const string UserId = "uid";     // redundante ao sub, mas útil se desejar
    public const string FullName = "name";    // compatível com ClaimTypes.Name
}
