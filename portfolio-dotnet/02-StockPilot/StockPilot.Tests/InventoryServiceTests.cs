using StockPilot.Core.Contracts;
using StockPilot.Core.Domain;
using StockPilot.Core.Exceptions;
using StockPilot.Core.Ports;
using StockPilot.Core.Services;
using Xunit;
namespace StockPilot.Tests;
public class InventoryServiceTests
{
    [Fact]
    public async Task SkuDuplicado_DeveRetornarConflito()
    {
        var repo = new MemoryRepo();
        var service = new InventoryService(repo);
        var req = new CreateProductRequest("ABC-1", "Luvas", 2, 1500);
        await service.CreateProductAsync(req, CancellationToken.None);
        await Assert.ThrowsAsync<ConflictException>(() => service.CreateProductAsync(req, CancellationToken.None));
    }
    private sealed class MemoryRepo : IInventoryRepository
    {
        private readonly List<Product> _products = [];
        public Task<bool> ExistsSkuAsync(string sku, CancellationToken ct) =>
            Task.FromResult(_products.Any(x => x.Sku == sku));
        public Task<Product?> GetAsync(Guid id, CancellationToken ct) => Task.FromResult(_products.FirstOrDefault(x => x.Id == id));
        public Task<IReadOnlyList<Product>> ListAsync(bool onlyLowStock, CancellationToken ct) =>
            Task.FromResult((IReadOnlyList<Product>)_products.Where(x => !onlyLowStock || x.IsLowStock).ToList());
        public Task AddProductAsync(Product product, CancellationToken ct) { _products.Add(product); return Task.CompletedTask; }
        public Task AddMovementAndSaveAsync(StockMovement movement, CancellationToken ct) => Task.CompletedTask;
        public Task<IReadOnlyList<StockMovement>> MovementsAsync(Guid productId, CancellationToken ct) =>
            Task.FromResult((IReadOnlyList<StockMovement>)new List<StockMovement>());
    }
}
