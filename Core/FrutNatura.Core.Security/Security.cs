namespace FrutNatura.Core.Security
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = "FrutNatura";
        public string Audience { get; set; } = "FrutNatura";
        public string Key { get; set; } = string.Empty;
        public int ExpiresMinutes { get; set; } = 120;
    }


}
