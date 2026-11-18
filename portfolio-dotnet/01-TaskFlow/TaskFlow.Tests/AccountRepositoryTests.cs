using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Domain;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Repositories;
using Xunit;
namespace TaskFlow.Tests;
public class AccountRepositoryTests
{
    [Fact]
    public async Task CriaEConsultaContaNoSqliteEmMemoria()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<TaskFlowDbContext>().UseSqlite(connection).Options;
        await using var context = new TaskFlowDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var repo = new AccountRepository(context);
        var account = new Account("Pessoa Teste", "demo@example.com");
        account.SetPasswordHash("hash-de-teste");
        await repo.AddAsync(account, CancellationToken.None);
        Assert.True(await repo.ExistsByEmailAsync("demo@example.com", CancellationToken.None));
        Assert.NotNull(await repo.GetByEmailAsync("demo@example.com", CancellationToken.None));
    }
}
