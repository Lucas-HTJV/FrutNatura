using Microsoft.AspNetCore.Mvc;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Auth;

namespace FrutNatura2.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var resp = await _auth.LoginAsync(request, ct);
        return Ok(resp);                 // <- não depende de Token/Success
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var resp = await _auth.RegisterAsync(request, ct);
        return Ok(resp);                 // <- idem
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var resp = await _auth.RefreshTokenAsync(request, ct);
        return Ok(resp);                 // <- idem
    }
}
