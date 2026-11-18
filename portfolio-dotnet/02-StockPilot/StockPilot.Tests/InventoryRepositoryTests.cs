using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StockPilot.Core.Domain;
using StockPilot.Infrastructure.Data;
using StockPilot.Infrastructure.Repositories;
using Xunit;
namespace StockPilot.Tests;
public class InventoryRepositoryTests
{
    [Fact]
    public async Task EntradaGravaSaldoEMovimentoNaMesmaOperacao()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<InventoryDbContext>().UseSqlite(connection).Options;
        await using var db = new InventoryDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var repo = new InventoryRepository(db);
        var product = new Product("M-01", "Teste de estoque", 2, 100);
        await repo.AddProductAsync(product, CancellationToken.None);
        product.ApplyMovement(MovementType.Entry, 5);
        await repo.AddMovementAndSaveAsync(new StockMovement(product.Id, MovementType.Entry, 5, "Teste"), CancellationToken.None);
        db.ChangeTracker.Clear();
        var fromDatabase = await repo.GetAsync(product.Id, CancellationToken.None);
        Assert.Equal(5, fromDatabase?.Stock);
        Assert.Single(await repo.MovementsAsync(product.Id, CancellationToken.None));
    }
}
