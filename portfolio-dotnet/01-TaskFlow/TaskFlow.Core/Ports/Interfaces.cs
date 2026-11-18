using TaskFlow.Core.Domain;
namespace TaskFlow.Core.Ports;
public interface IAccountRepository
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
    Task<Account?> GetByEmailAsync(string email, CancellationToken ct);
    Task AddAsync(Account account, CancellationToken ct);
}
public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken ct);
    Task<TaskItem?> GetOwnedAsync(Guid taskId, Guid ownerId, CancellationToken ct);
    Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(Guid ownerId, WorkStatus? status,
        int page, int pageSize, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);
    void Delete(TaskItem task);
}
public interface IPasswordService
{
    string Hash(Account account, string password);
    bool Verify(Account account, string password);
}
public interface ITokenService
{
    (string Token, DateTime ExpiresAtUtc) Issue(Account account);
}
