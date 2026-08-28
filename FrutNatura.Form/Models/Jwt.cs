using System;
using System.Text.Json;

namespace FrutNatura.Desktop.Models
{
    public static class Jwt
    {
        private static string B64(string s)
            => s.Replace('-', '+').Replace('_', '/')
                .PadRight(s.Length + (4 - s.Length % 4) % 4, '=');

        public static string? GetClaim(string jwt, string claim)
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2) return null;
            var payloadJson = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(B64(parts[1])));
            using var doc = JsonDocument.Parse(payloadJson);
            return doc.RootElement.TryGetProperty(claim, out var v) ? v.ToString() : null;
        }
    }
}
