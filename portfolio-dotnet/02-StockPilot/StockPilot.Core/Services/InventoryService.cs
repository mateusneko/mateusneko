using StockPilot.Core.Contracts;
using StockPilot.Core.Domain;
using StockPilot.Core.Exceptions;
using StockPilot.Core.Ports;
namespace StockPilot.Core.Services;
public sealed class InventoryService(IInventoryRepository repository)
{
    private static ProductResponse Map(Product p) =>
        new(p.Id, p.Sku, p.Name, p.Stock, p.MinimumStock, p.UnitPriceCents, p.IsLowStock);
    private static MovementResponse Map(StockMovement m) =>
        new(m.Id, m.ProductId, m.Type, m.Quantity, m.Reason, m.CreatedAtUtc);
    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest input, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(input.Sku)) throw new BusinessRuleException("Informe o SKU.");
        var sku = input.Sku.Trim().ToUpperInvariant();
        if (await repository.ExistsSkuAsync(sku, ct)) throw new ConflictException("SKU já cadastrado.");
        var product = new Product(sku, input.Name, input.MinimumStock, input.UnitPriceCents);
        await repository.AddProductAsync(product, ct);
        return Map(product);
    }
    public async Task<ProductResponse> GetAsync(Guid id, CancellationToken ct) =>
        Map(await repository.GetAsync(id, ct) ?? throw new NotFoundException("Produto não encontrado."));
    public async Task<IReadOnlyList<ProductResponse>> ListAsync(bool lowStock, CancellationToken ct) =>
        (await repository.ListAsync(lowStock, ct)).Select(Map).ToList();
    public async Task<MovementResponse> MoveAsync(Guid id, CreateMovementRequest input, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(input.Reason) || input.Reason.Length > 300)
            throw new BusinessRuleException("Justificativa obrigatória (até 300 caracteres).");
        var product = await repository.GetAsync(id, ct) ?? throw new NotFoundException("Produto não encontrado.");
        product.ApplyMovement(input.Type, input.Quantity);
        var movement = new StockMovement(product.Id, input.Type, input.Quantity, input.Reason);
        await repository.AddMovementAndSaveAsync(movement, ct);
        return Map(movement);
    }
    public async Task<IReadOnlyList<MovementResponse>> HistoryAsync(Guid id, CancellationToken ct)
    {
        _ = await repository.GetAsync(id, ct) ?? throw new NotFoundException("Produto não encontrado.");
        return (await repository.MovementsAsync(id, ct)).Select(Map).ToList();
    }
}
