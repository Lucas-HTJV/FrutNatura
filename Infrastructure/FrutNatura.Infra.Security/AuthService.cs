using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Security;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Auth;
using FrutNatura.Core.Domain;
using FrutNatura.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrutNatura.Infra.Security.Auth
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUsuariosRepository _usuarios;
        private readonly IRefreshTokensService _refreshTokens;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uow;

        public AuthService(
            IUsuariosRepository usuarios,
            IRefreshTokensService refreshTokens,
            IPasswordHasher hasher,
            ITokenService tokenService,
            IUnitOfWork uow)
        {
            _usuarios = usuarios;
            _refreshTokens = refreshTokens;
            _hasher = hasher;
            _tokenService = tokenService;
            _uow = uow;
        }

        // ========================
        // REGISTRO DE USUÁRIO
        // ========================
        public async Task<AuthResult> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
        {
            var exists = await _usuarios.GetByEmailAsync(req.Email, ct);
            if (exists is not null)
                return new AuthResult(false, "E-mail já cadastrado.");

            if (string.IsNullOrWhiteSpace(req.HashPassword))
                return new AuthResult(false, "Senha obrigatória.");

            // Gera o hash da senha
            var hash = _hasher.HashPassword(req.HashPassword);

            // Cria o usuário
            var usuario = Usuario.Criar(req.Nome, req.Email, hash, new[] { "Cliente" });

            // Persiste no banco
            await _usuarios.AddAsync(usuario, ct);
            await _uow.SaveChangesAsync(ct);

            return new AuthResult(true, null);
        }

        // ========================
        // LOGIN
        // ========================
        public async Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
        {
            var user = await _usuarios.GetByEmailAsync(req.Email, ct);
            if (user is null)
                return new LoginResponse(string.Empty, false, "Usuário não encontrado.");

            if (string.IsNullOrWhiteSpace(req.HashPassword))
                return new LoginResponse(string.Empty, false, "Senha obrigatória.");

            // Verifica o hash
            var senhaCorreta = _hasher.VerifyHashedPassword(user.PasswordHash, req.HashPassword);
            if (!senhaCorreta)
                return new LoginResponse(string.Empty, false, "Credenciais inválidas.");

            // Gera token JWT
            var roles = user.Roles?.ToList() ?? new List<string> { "Cliente" };
            var token = _tokenService.CreateAccessToken(user.Id, user.Email.Valor, roles, ct);

            // Cria e salva o refresh token
            var refresh = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UsuarioId = user.Id,
                Token = Guid.NewGuid().ToString("N"), 
                CreatedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(7),
                RevokedUtc = null
            };

            await _refreshTokens.AddAsync(refresh, ct);
            await _uow.SaveChangesAsync(ct);

            var rolePrincipal = roles.FirstOrDefault() ?? "Cliente";

            return new LoginResponse(
                AccessToken: token,
                Success: true,
                Error: null,
                RefreshToken: refresh.Token,
                UsuarioId: user.Id,
                Name: user.Nome,
                Role: rolePrincipal
            );

        }

        // ========================
        // REFRESH TOKEN
        // ========================
        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest req, CancellationToken ct = default)
        {
            var token = await _refreshTokens.GetByTokenAsync(req.RefreshToken, ct);
            if (token is null)
                return new RefreshTokenResponse(string.Empty, false, "Token inválido.");

            var user = await _usuarios.GetAsync(token.UsuarioId, ct);
            if (user is null)
                return new RefreshTokenResponse(string.Empty, false, "Usuário não encontrado.");

            var roles = user.Roles?.ToList() ?? new List<string> { "Cliente" };
            var accessToken = _tokenService.CreateAccessToken(user.Id, user.Email.Valor, roles, ct);
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UsuarioId = user.Id, 
                Token = Guid.NewGuid().ToString("N"),
                CreatedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(7),
                RevokedUtc = null
            };

            return new RefreshTokenResponse(accessToken, true);
        }
    }
}
