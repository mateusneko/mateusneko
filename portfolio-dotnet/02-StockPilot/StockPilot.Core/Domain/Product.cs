using StockPilot.Core.Exceptions;
namespace StockPilot.Core.Domain;
public enum MovementType { Entry, Exit }
public sealed class Product
{
    private Product() { }
    public Product(string sku, string name, int minimumStock, long unitPriceCents)
    {
        if (string.IsNullOrWhiteSpace(sku) || sku.Length > 40) throw new BusinessRuleException("SKU obrigatório (até 40 caracteres).");
        if (string.IsNullOrWhiteSpace(name) || name.Length > 160) throw new BusinessRuleException("Nome obrigatório (até 160 caracteres).");
        if (minimumStock < 0 || unitPriceCents < 0) throw new BusinessRuleException("Estoque mínimo e preço não podem ser negativos.");
        Id = Guid.NewGuid();
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        MinimumStock = minimumStock;
        UnitPriceCents = unitPriceCents;
        Version = Guid.NewGuid().ToString("N");
    }
    public Guid Id { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int Stock { get; private set; }
    public int MinimumStock { get; private set; }
    public long UnitPriceCents { get; private set; }
    // Token de concorrência otimista, configurado no EF Core.
    public string Version { get; private set; } = string.Empty;
    public bool IsLowStock => Stock <= MinimumStock;
    public void ApplyMovement(MovementType type, int quantity)
    {
        if (!Enum.IsDefined(type)) throw new BusinessRuleException("Tipo de movimentação inválido.");
        if (quantity <= 0) throw new BusinessRuleException("Quantidade deve ser maior que zero.");
        if (type == MovementType.Exit && quantity > Stock)
            throw new BusinessRuleException("Saída não permitida: saldo insuficiente.");
        checked { Stock += type == MovementType.Entry ? quantity : -quantity; }
        Version = Guid.NewGuid().ToString("N");
    }
}
