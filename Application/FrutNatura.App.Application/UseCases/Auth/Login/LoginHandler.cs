using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Security;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Auth;
using MediatR;
using System.Collections.Generic;

namespace FrutNatura.App.Application.UseCases.Auth.Login;


public sealed class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUsuariosRepository _usuarios;
    private readonly ITokenService _tokens;
    private readonly IPasswordHasher _hasher;

    public LoginHandler(IUsuariosRepository usuarios, ITokenService tokens, IPasswordHasher hasher)
    {
        _usuarios = usuarios;
        _tokens = tokens;
        _hasher = hasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand cmd, CancellationToken ct)
    {
        // 1) Localiza o usuário pelo e-mail
        var user = await _usuarios.GetByEmailAsync(cmd.Email, ct);
        if (user is null)
            throw new UnauthorizedAccessException("Usuário não encontrado.");

        // 2) Valida senha
        if (string.IsNullOrEmpty(user.PasswordHash) ||
            !_hasher.VerifyHashedPassword(user.PasswordHash, cmd.Password))
        {
            throw new UnauthorizedAccessException("Senha incorreta.");
        }

        // 3) Emite o JWT com as roles do usuário
        var access = _tokens.CreateAccessToken(user.Id, user.Email.ToString(), user.Roles);

        // 4) Retorna o contrato esperado pela API
        return new LoginResponse(access);
    }
}
