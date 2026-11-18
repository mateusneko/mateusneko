namespace TaskFlow.Core.Domain;
public sealed class Account
{
    private Account() { } // EF Core
    public Account(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
    }
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public void SetPasswordHash(string hash) => PasswordHash = hash;
}
