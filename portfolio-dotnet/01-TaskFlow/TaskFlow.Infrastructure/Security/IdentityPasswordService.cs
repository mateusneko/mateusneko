using Microsoft.AspNetCore.Identity;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Ports;
namespace TaskFlow.Infrastructure.Security;
public sealed class IdentityPasswordService : IPasswordService
{
    private readonly PasswordHasher<Account> _hasher = new();
    public string Hash(Account account, string password) => _hasher.HashPassword(account, password);
    public bool Verify(Account account, string password) =>
        _hasher.VerifyHashedPassword(account, account.PasswordHash, password) != PasswordVerificationResult.Failed;
}
