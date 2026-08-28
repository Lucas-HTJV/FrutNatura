using System;
using System.Text.Json.Serialization;

namespace FrutNatura.Form.Models
{
    public sealed class LoginResponse
    {
        // nomes camelCase para casar com o JSON da API
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
    }
}
