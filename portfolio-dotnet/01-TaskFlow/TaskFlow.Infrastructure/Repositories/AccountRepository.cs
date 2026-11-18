using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Ports;
using TaskFlow.Infrastructure.Data;
namespace TaskFlow.Infrastructure.Repositories;
public sealed class AccountRepository(TaskFlowDbContext db) : IAccountRepository
{
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct) =>
        db.Accounts.AnyAsync(x => x.Email == email, ct);
    public Task<Account?> GetByEmailAsync(string email, CancellationToken ct) =>
        db.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, ct);
    public async Task AddAsync(Account account, CancellationToken ct)
    {
        db.Accounts.Add(account);
        await db.SaveChangesAsync(ct);
    }
}
