using Microsoft.AspNetCore.Mvc;
using TaskFlow.Core.Contracts;
using TaskFlow.Core.Services;
namespace TaskFlow.Api.Controllers;
[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest input, CancellationToken ct) =>
        Ok(await auth.RegisterAsync(input, ct));
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest input, CancellationToken ct) =>
        Ok(await auth.LoginAsync(input, ct));
}
