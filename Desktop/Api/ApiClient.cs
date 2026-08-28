using FrutNatura.Desktop.Models;
using FrutNatura.Desktop.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrutNatura.Desktop.Api
{
    public sealed class ApiClient
    {
        private readonly HttpClient _http;          // NÃO anulável
        private readonly string _baseUrl;

        public static readonly JsonSerializerOptions Json = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Construtor com URL base (este é o que estamos usando no Program.cs)
        public ApiClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("BaseUrl inválida.", nameof(baseUrl));

            if (!baseUrl.EndsWith("/"))
                baseUrl += "/";

            _baseUrl = baseUrl;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };

            _http = new HttpClient(handler)
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        // Construtor recebendo um HttpClient pronto (se um dia você quiser usar DI)
        public ApiClient(HttpClient httpClient, string baseUrl = "")
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUrl = baseUrl;
        }

        // Construtor padrão (se ainda for usado em algum lugar)
        // Usa a URL padrão da sua API
        public ApiClient() : this("https://localhost:7094/") { }

        // Método para aplicar o Bearer Token
        private void ApplyBearer()
        {
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(SessionManager.AccessToken))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        SessionManager.AccessToken
                    );
            }
        }

        // Método para garantir que a resposta seja bem-sucedida
        private static async Task EnsureSuccessAsync(HttpResponseMessage res)
        {
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new HttpRequestException(
                    $"HTTP {(int)res.StatusCode} ({res.ReasonPhrase}). Body: {body}"
                );
            }
        }

        // =========================
        // AUTH - Login e Refresh
        // =========================

        public async Task<LoginResponse?> LoginAsync(LoginRequest req)
        {
            var res = await _http.PostAsJsonAsync("/api/auth/login", req, Json)
                                 .ConfigureAwait(false);
            await EnsureSuccessAsync(res);

            var data = await res.Content.ReadFromJsonAsync<LoginResponse>(Json)
                                       .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(data?.AccessToken))
            {
                SessionManager.AccessToken = data.AccessToken;
                SessionManager.RefreshToken = data.RefreshToken;
                SessionManager.UserName = data.Name;
                SessionManager.UserRole = data.Role;                
                SessionManager.UsuarioId = data.UsuarioId;

                ApplyBearer();
            }

            return data;
        }


        public async Task<LoginResponse?> RefreshAsync(string refreshToken)
        {
            var body = new { refreshToken };
            var res = await _http.PostAsJsonAsync("/api/auth/refresh-token", body, Json).ConfigureAwait(false);
            await EnsureSuccessAsync(res);

            var data = await res.Content.ReadFromJsonAsync<LoginResponse>(Json).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(data?.AccessToken))
            {
                SessionManager.AccessToken = data.AccessToken;
                ApplyBearer();
            }

            return data;
        }

        // =========================
        // CHAMADOS - Listar e Detalhes
        // =========================

        public async Task<List<ChamadoDto>> ListarChamadosAsync(string? status = null)
        {
            ApplyBearer();

            var url = "api/staff/chamados";
            if (!string.IsNullOrWhiteSpace(status))
                url += $"?status={Uri.EscapeDataString(status)}";

            try
            {
                var response = await _http
                    .GetFromJsonAsync<ApiResponse<List<ChamadoDto>>>(url, Json)
                    .ConfigureAwait(false);

                return response?.Items ?? new List<ChamadoDto>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro de JSON: {ex.Message}");
                throw;
            }
        }

        // Método para obter os detalhes de um chamado específico
        public async Task<ChamadoDto> ObterChamadoAsync(Guid id)
        {
            ApplyBearer();

            try
            {
                var response = await _http
                    .GetFromJsonAsync<ChamadoDto>($"api/staff/chamados/{id}", Json)
                    .ConfigureAwait(false);

                if (response == null)
                    throw new Exception("Chamado não encontrado.");

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
                throw new Exception("Erro ao obter os detalhes do chamado.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao processar a resposta: {ex.Message}");
                throw new Exception("Erro ao processar os dados do chamado.");
            }
        }

        public async Task<Guid> ObterIdResponsavelPorNome(string nomeResponsavel)
        {
            ApplyBearer();

            var response = await _http.GetAsync($"/api/responsavel/id?nome={Uri.EscapeDataString(nomeResponsavel)}")
                                      .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responsavelId = await response.Content.ReadFromJsonAsync<Guid>().ConfigureAwait(false);
                return responsavelId;
            }

            throw new Exception("Erro ao buscar ID do responsável.");
        }

        // =========================
        // IA / MENSAGENS
        // =========================


        public async Task<string> ChamarIAAsync(Guid chamadoId, string descricaoChamado)
        {
            ApplyBearer();


            var body = new
            {
                chamadoId = chamadoId,
                message = descricaoChamado
            };

           
            var response = await _http
                .PostAsJsonAsync("api/chat", body, Json) 
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception($"Erro ao chamar a IA (HTTP {(int)response.StatusCode}): {erro}");
            }

            
            var chatResponse = await response.Content
                .ReadFromJsonAsync<ChatResponseDto>(Json)
                .ConfigureAwait(false);

            return chatResponse?.Reply ?? string.Empty;
        }

       
        private sealed class ChatResponseDto
        {
            public string Reply { get; set; } = string.Empty;
        }


        public async Task<List<MensagemDto>> ObterMensagensAsync(Guid chamadoId)
        {
            ApplyBearer();

            var response = await _http.GetAsync($"/api/chamados/{chamadoId}/mensagens")
                                      .ConfigureAwait(false);
           
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("JSON recebido: " + json);

            if (response.IsSuccessStatusCode)
            {
                // Aqui já dá pra usar ReadFromJsonAsync direto
                var mensagens = await response.Content
                    .ReadFromJsonAsync<List<MensagemDto>>(Json)
                    .ConfigureAwait(false);

                return mensagens ?? new List<MensagemDto>();
            }

            throw new Exception("Erro ao obter mensagens.");
        }

        public async Task EnviarMensagemAsync(Guid chamadoId, NovaMensagemRequest req)
        {
            ApplyBearer();

            var res = await _http
                .PostAsJsonAsync($"/api/chamados/{chamadoId}/mensagens", req, Json)
                .ConfigureAwait(false);

            await EnsureSuccessAsync(res);
        }

        // =========================
        // Atribuição de Responsável
        // =========================

        public async Task AtribuirResponsavelAsync(Guid chamadoId, Guid responsavelId)
        {
            ApplyBearer(); // aplica o Bearer Token

            // ROTA IGUAL AO SWAGGER
            var url = $"api/staff/chamados/{chamadoId}/atribuir";

            // Corpo da requisição - use o mesmo nome que o endpoint espera
            var body = new { ResponsavelId = responsavelId };
            // se no Swagger/DTO estiver "responsavelId", troque para:
            // var body = new { responsavelId = responsavelId };

            // AQUI PRECISA SER PUT, NÃO PATCH, NEM POST:
            var res = await _http.PutAsJsonAsync(url, body, Json).ConfigureAwait(false);

            await EnsureSuccessAsync(res); // vai lançar HttpRequestException se não for 2xx
        }


        // =========================
        // Métodos ainda não usados
        // =========================

        internal Task<ChamadoDto> GetChamadoAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        internal Task SaveChamadoAsync(ChamadoDto chamado)
        {
            throw new NotImplementedException();
        }

        internal async Task<ChamadoDetalheDto?> GetChamadoDetailsAsync(Guid id)
        {
            var response = await _http
                .GetFromJsonAsync<ChamadoDetalheDto>($"api/staff/chamados/{id}", Json)
                .ConfigureAwait(false);

            return response;
        }
    }

    
    public class IAService
    {
        private readonly ApiClient _apiClient;

        public IAService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // Método para obter a resposta da IA
        public async Task<string> ObterRespostaIAAsync(Guid chamadoId, string texto)
        {
            try
            {
                var resposta = await _apiClient.ChamarIAAsync(chamadoId, texto);
                return resposta ?? "Desculpe, não conseguimos obter a resposta da IA.";
            }
            catch (Exception ex)
            {
                return $"Erro ao obter resposta da IA: {ex.Message}";
            }
        }
    }
}
