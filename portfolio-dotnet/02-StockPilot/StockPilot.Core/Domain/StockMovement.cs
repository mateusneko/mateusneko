namespace StockPilot.Core.Domain;
public sealed class StockMovement
{
    private StockMovement() { }
    public StockMovement(Guid productId, MovementType type, int quantity, string reason)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Type = type;
        Quantity = quantity;
        Reason = reason.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public MovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
}
