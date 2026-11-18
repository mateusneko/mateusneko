using Microsoft.EntityFrameworkCore;
using StockPilot.Core.Domain;
using StockPilot.Core.Exceptions;
using StockPilot.Core.Ports;
using StockPilot.Infrastructure.Data;
namespace StockPilot.Infrastructure.Repositories;
public sealed class InventoryRepository(InventoryDbContext db) : IInventoryRepository
{
    public Task<bool> ExistsSkuAsync(string sku, CancellationToken ct) =>
        db.Products.AnyAsync(x => x.Sku == sku, ct);
    public Task<Product?> GetAsync(Guid id, CancellationToken ct) =>
        db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<Product>> ListAsync(bool lowStock, CancellationToken ct)
    {
        var query = db.Products.AsNoTracking();
        if (lowStock) query = query.Where(x => x.Stock <= x.MinimumStock);
        return await query.OrderBy(x => x.Name).ToListAsync(ct);
    }
    public async Task AddProductAsync(Product product, CancellationToken ct)
    {
        db.Products.Add(product);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException) { throw new ConflictException("Não foi possível salvar o produto. Confira o SKU."); }
    }
    public async Task AddMovementAndSaveAsync(StockMovement movement, CancellationToken ct)
    {
        // O EF Core salva alteração do saldo e movimento na mesma transação.
        db.Movements.Add(movement);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Saldo alterado por outro usuário. Atualize e tente novamente.");
        }
    }
    public async Task<IReadOnlyList<StockMovement>> MovementsAsync(Guid id, CancellationToken ct) =>
        await db.Movements.AsNoTracking().Where(x => x.ProductId == id)
            .OrderByDescending(x => x.CreatedAtUtc).Take(200).ToListAsync(ct);
}
