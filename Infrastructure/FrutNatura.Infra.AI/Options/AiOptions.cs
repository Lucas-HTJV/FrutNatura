namespace FrutNatura.Infra.AI.Options;

public sealed class AiOptions
{
    /// <summary>Provider: "Echo" (padrão) ou "OpenAI".</summary>
    public string Provider { get; set; } = "Echo";

    /// <summary>Modelo base do provedor (ex.: "gpt-4o-mini" / "gpt-4.1-mini" / "gpt-35-turbo").</summary>
    public string? Model { get; set; }

    /// <summary>API Key para o provedor (se usar OpenAI/Azure OpenAI).</summary>
    public string? ApiKey { get; set; }

    /// <summary>Endpoint base (necessário em Azure OpenAI ou proxy compatível). Ex.: https://{host}/openai</summary>
    public string? Endpoint { get; set; }

    /// <summary>Caminho do endpoint de completions (se diferente). Ex.: "/v1/chat/completions"</summary>
    public string CompletionsPath { get; set; } = "/v1/chat/completions";

    /// <summary>Timeout em segundos para chamadas HTTP.</summary>
    public int TimeoutSeconds { get; set; } = 30;
}
