// Arquivo: ApiClient.cs
// Projeto: FrutNatura.Form (WinForms)

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FrutNatura.Form.Models; // seus DTOs/Enums locais (REST)

namespace FrutNatura.Form
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions;
        private string? _token;
        private Guid _currentUserId;

        public string BaseUrl { get; set; }  // pode ser setado externamente (compat)
        public string? Token => _token;      // leitura (se alguma tela usa)

        public ApiClient(string baseUrl)
        {
            BaseUrl = baseUrl.TrimEnd('/');
            _http = new HttpClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        // construtor compat (sem args)
        public ApiClient() : this("https://localhost:7094") { }

        // ---------------------- Auth ----------------------
        public async Task<bool> LoginAsync(string email, string senha)
        {
            var payload = new { email, senha };
            var response = await _http.PostAsJsonAsync($"{BaseUrl}/api/auth/login", payload);

            if (!response.IsSuccessStatusCode) return false;

            var data = await response.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);
            if (data == null || string.IsNullOrWhiteSpace(data.Token)) return false;

            _token = data.Token;
            _currentUserId = data.UserId;
            EnsureAuthHeader();
            return true;
        }

        public void Logout()
        {
            _token = null;
            _currentUserId = Guid.Empty;
            _http.DefaultRequestHeaders.Authorization = null;
        }

        private void EnsureAuthHeader()
        {
            if (string.IsNullOrWhiteSpace(_token))
                throw new InvalidOperationException("Usuário não autenticado. Faça login primeiro.");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }

        public void SetUser(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId inválido.", nameof(userId));
            _currentUserId = userId;
        }

        // ------------------ Chamados (Staff) ------------------
        public async Task<PagedResult<ChamadoDto>> ListarChamadosAsync(
            string? status = null,
            string? escopo = null,
            Guid? responsavelId = null,
            int page = 1,
            int pageSize = 20)
        {
            EnsureAuthHeader();

            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(status)) query.Add($"status={Uri.EscapeDataString(status)}");
            if (!string.IsNullOrWhiteSpace(escopo)) query.Add($"escopo={Uri.EscapeDataString(escopo)}");
            if (responsavelId.HasValue && responsavelId.Value != Guid.Empty) query.Add($"responsavelId={responsavelId}");
            query.Add($"page={page}");
            query.Add($"pageSize={pageSize}");

            var url = $"{BaseUrl}/api/staff/chamados" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<PagedResult<ChamadoDto>>(_jsonOptions);
            return data ?? new PagedResult<ChamadoDto>(new List<ChamadoDto>(), 0, page, pageSize);
        }

        public async Task<ChamadoDto?> ObterChamadoAsync(Guid chamadoId)
        {
            EnsureAuthHeader();

            var response = await _http.GetAsync($"{BaseUrl}/api/chamados/{chamadoId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ChamadoDto>(_jsonOptions);
        }

        public async Task<bool> AtribuirParaMimAsync(Guid chamadoId)
        {
            EnsureAuthHeader();

            if (_currentUserId == Guid.Empty)
                throw new InvalidOperationException("UserId não definido. Use SetUser(userId) após o login.");

            var body = new { responsavelId = _currentUserId };
            var resp = await _http.PostAsJsonAsync($"{BaseUrl}/api/staff/chamados/{chamadoId}/atribuir", body);
            return resp.IsSuccessStatusCode;
        }

        // compat com código antigo que passava o ID do responsável explicitamente
        public async Task<bool> AtribuirChamadoAsync(Guid chamadoId, Guid responsavelId)
        {
            EnsureAuthHeader();
            var body = new { responsavelId };
            var resp = await _http.PostAsJsonAsync($"{BaseUrl}/api/staff/chamados/{chamadoId}/atribuir", body);
            return resp.IsSuccessStatusCode;
        }

        // endpoint do seu controller staff: POST {id}/status recebendo StatusChamado no body
        public async Task<bool> AlterarStatusAsync(Guid chamadoId, string novoStatus)
        {
            EnsureAuthHeader();
            var resp = await _http.PostAsJsonAsync($"{BaseUrl}/api/staff/chamados/{chamadoId}/status", novoStatus);
            return resp.IsSuccessStatusCode;
        }

        // 🔹 overload para aceitar o enum diretamente (recomendado no form)
        public Task<bool> AlterarStatusAsync(Guid chamadoId, StatusChamado novoStatus)
            => AlterarStatusAsync(chamadoId, novoStatus.ToString());

        // compat com nome antigo
        public Task<bool> AlterarStatusChamadoAsync(Guid chamadoId, string novoStatus)
            => AlterarStatusAsync(chamadoId, novoStatus);

        // ------------------ Mensagens ------------------
        public async Task<List<MensagemDto>> ListarMensagensAsync(Guid chamadoId)
        {
            EnsureAuthHeader();

            var response = await _http.GetAsync($"{BaseUrl}/api/mensagens/{chamadoId}");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<List<MensagemDto>>(_jsonOptions);
            return data ?? new List<MensagemDto>();
        }

        public async Task<bool> EnviarMensagemAsync(Guid chamadoId, string conteudo)
        {
            EnsureAuthHeader();

            var payload = new { chamadoId, conteudo, autorId = _currentUserId };
            var response = await _http.PostAsJsonAsync($"{BaseUrl}/api/mensagens", payload);
            return response.IsSuccessStatusCode;
        }

        // ------------------ Compat: nomes antigos ------------------
        public Task<ChamadoDto?> ObterChamadoPorIdAsync(Guid chamadoId)
            => ObterChamadoAsync(chamadoId);
    }
}
