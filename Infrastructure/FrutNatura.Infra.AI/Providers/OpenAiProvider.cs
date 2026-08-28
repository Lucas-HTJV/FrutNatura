using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra.AI.Options;
using Microsoft.Extensions.Options;

namespace FrutNatura.Infra.AI;

public sealed class OpenAiProvider : IAIProvider
{
    private readonly HttpClient _http;
    private readonly AiOptions _opt;

    public OpenAiProvider(HttpClient http, IOptions<AiOptions> opt)
    {
        _http = http;
        _opt = opt.Value;

        // Configurações padrão seguras
        if (string.IsNullOrWhiteSpace(_opt.Endpoint))
            _opt.Endpoint = "https://api.openai.com"; // HTTPS padrão
        if (string.IsNullOrWhiteSpace(_opt.CompletionsPath))
            _opt.CompletionsPath = "/v1/chat/completions";

        // Força uso de HTTPS
        if (!_opt.Endpoint.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            _opt.Endpoint = _opt.Endpoint.Replace("http://", "https://");
    }

    public async Task<string> CompleteAsync(string prompt, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_opt.ApiKey))
            return "[openai] API key não configurada.";

        if (string.IsNullOrWhiteSpace(_opt.Model))
            _opt.Model = "gpt-4o-mini";

        var url = $"{_opt.Endpoint.TrimEnd('/')}{_opt.CompletionsPath}";
        using var req = new HttpRequestMessage(HttpMethod.Post, url);

        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _opt.ApiKey);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var body = new
        {
            model = _opt.Model,
            messages = new object[]
            {
                new { role = "system", content = "Você é um atendente simpático do hortifrúti FrutNatura." },
                new { role = "user", content = prompt }
            },
            temperature = 0.4
        };

        req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        using var resp = await _http.SendAsync(req, ct);
        var json = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            return $"[openai] erro HTTP {(int)resp.StatusCode}: {resp.ReasonPhrase}";
        }

        try
        {
            using var doc = JsonDocument.Parse(json);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content ?? "[openai] sem conteúdo na resposta.";
        }
        catch (Exception ex)
        {
            return $"[openai] falha ao ler a resposta ({ex.Message}).";
        }
    }
}
