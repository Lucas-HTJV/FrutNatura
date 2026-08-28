using System;
using System.Text.Json.Serialization;

namespace FrutNatura.Desktop.Models
{
    // DTO usado para receber as mensagens da API (GET /api/chamados/{chamadoId}/mensagens)
    public sealed class MensagemDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("chamadoId")]
        public Guid ChamadoId { get; set; }

        [JsonPropertyName("autorId")]
        public Guid AutorId { get; set; }

        // A RESPOSTA DA API USA "texto"
        [JsonPropertyName("texto")]
        public string Texto { get; set; } = string.Empty;

        [JsonPropertyName("criadoEmUtc")]
        public DateTime CriadoEmUtc { get; set; }
    }

    // DTO usado para ENVIAR mensagens para a API (POST /api/chamados/{chamadoId}/mensagens)
    public sealed class NovaMensagemRequest
    {
        // O Swagger mostra exatamente estes 3 campos no body:
        // { "autorId": ..., "conteudo": "...", "visivelParaCliente": true }
        [JsonPropertyName("autorId")]
        public Guid AutorId { get; set; }

        [JsonPropertyName("conteudo")]
        public string Conteudo { get; set; } = string.Empty;

        [JsonPropertyName("visivelParaCliente")]
        public bool VisivelParaCliente { get; set; } = true;
    }
}
