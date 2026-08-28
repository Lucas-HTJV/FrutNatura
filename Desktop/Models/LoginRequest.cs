using System.Text.Json.Serialization;

namespace FrutNatura.Desktop.Models
{
    public sealed class LoginRequest
    {
        [JsonPropertyName("email")] public string Email { get; set; } = "";
        [JsonPropertyName("hashPassword")] public string HashPassword { get; set; } = "";
    }
}
