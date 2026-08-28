using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Auth;

namespace FrutNatura.Funcs.Auth.Login;

public sealed class LoginFunction
{
    private readonly IAuthService _auth;
    public LoginFunction(IAuthService auth) => _auth = auth;

    public Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken ct = default)
        => _auth.LoginAsync(request, ct);
}
