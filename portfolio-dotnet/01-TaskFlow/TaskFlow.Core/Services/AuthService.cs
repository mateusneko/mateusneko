using TaskFlow.Core.Contracts;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Ports;
namespace TaskFlow.Core.Services;
public sealed class AuthService(IAccountRepository accounts, IPasswordService passwords, ITokenService tokens)
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
            throw new BusinessRuleException("Nome, e-mail e senha de no mínimo 8 caracteres são obrigatórios.");
        var email = request.Email.Trim().ToLowerInvariant();
        if (await accounts.ExistsByEmailAsync(email, ct))
            throw new ConflictException("E-mail já cadastrado.");
        var account = new Account(request.Name, email);
        account.SetPasswordHash(passwords.Hash(account, request.Password));
        await accounts.AddAsync(account, ct);
        var (token, expires) = tokens.Issue(account);
        return new AuthResponse(token, "Bearer", expires);
    }
    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var account = await accounts.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), ct);
        if (account is null || !passwords.Verify(account, request.Password))
            throw new BusinessRuleException("Credenciais inválidas.");
        var (token, expires) = tokens.Issue(account);
        return new AuthResponse(token, "Bearer", expires);
    }
}
