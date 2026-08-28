using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Auth;

namespace FrutNatura.Funcs.Auth.Register;

public sealed class RegisterFunction
{
    private readonly IAuthService _auth;
    public RegisterFunction(IAuthService auth) => _auth = auth;

    public Task<AuthResult> HandleAsync(RegisterRequest request, CancellationToken ct = default)
        => _auth.RegisterAsync(request, ct);
}
