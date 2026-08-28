using System;
using System.Text.Json.Serialization;

namespace FrutNatura.Desktop.Models
{
    public sealed class LoginResponse
    {
        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("usuarioId")]
        public Guid UsuarioId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }
}
