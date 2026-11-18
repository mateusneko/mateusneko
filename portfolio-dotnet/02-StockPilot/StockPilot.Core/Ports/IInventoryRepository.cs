using StockPilot.Core.Domain;
namespace StockPilot.Core.Ports;
public interface IInventoryRepository
{
    Task<bool> ExistsSkuAsync(string sku, CancellationToken ct);
    Task<Product?> GetAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Product>> ListAsync(bool onlyLowStock, CancellationToken ct);
    Task AddProductAsync(Product product, CancellationToken ct);
    Task AddMovementAndSaveAsync(StockMovement movement, CancellationToken ct);
    Task<IReadOnlyList<StockMovement>> MovementsAsync(Guid productId, CancellationToken ct);
}
