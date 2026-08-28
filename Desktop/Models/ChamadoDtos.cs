using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FrutNatura.Desktop.Models
{
    // Modelo para Chamado
    public sealed class ChamadoDto
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }
        [JsonPropertyName("clienteId")] public Guid ClienteId { get; set; }
        [JsonPropertyName("RefleshToken")] public string RefleshToken { get; set; }
       
        [JsonPropertyName("titulo")] public string Titulo { get; set; } = "";
        [JsonPropertyName("descricao")] public string Descricao { get; set; } = "";
        [JsonPropertyName("texto")] public string Conteudo { get; set; } = "";
        [JsonPropertyName("status")] public string Status { get; set; } = "";
        [JsonPropertyName("prioridade")] public string Prioridade { get; set; } = "";

        [JsonPropertyName("criadoEmUtc")] public DateTime CriadoEmUtc { get; set; }
        [JsonPropertyName("responsavelId")] public Guid? ResponsavelId { get; set; }
        [JsonPropertyName("fechadoEmUtc")] public DateTime? FechadoEmUtc { get; set; }

        [JsonPropertyName("clienteNome")] public string ClienteNome { get; set; } = "";

    }

    // Modelo detalhado para Chamado
    public sealed class ChamadoDetalheDto
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }

        // Corrigir o nome do campo, caso seja realmente "ClienteNome" ou "ClienteId"
        [JsonPropertyName("clienteNome")] public string? ClienteNome { get; set; }

        [JsonPropertyName("titulo")] public string Titulo { get; set; } = "";
        [JsonPropertyName("descricao")] public string Descricao { get; set; } = "";

        // A lista de mensagens associadas ao chamado
        [JsonPropertyName("status")] public string Status { get; set; } = ""; // Corrigir para string
        [JsonPropertyName("mensagens")] public List<MensagemDto> Mensagens { get; set; } = new();

       
    }
}
