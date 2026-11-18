using System.ComponentModel.DataAnnotations;
using StockPilot.Core.Domain;
namespace StockPilot.Core.Contracts;
public sealed record CreateProductRequest(
    [property: Required, StringLength(40)] string Sku,
    [property: Required, StringLength(160)] string Name,
    [property: Range(0, int.MaxValue)] int MinimumStock,
    [property: Range(0, long.MaxValue)] long UnitPriceCents);
public sealed record CreateMovementRequest(MovementType Type,
    [property: Range(1, int.MaxValue)] int Quantity,
    [property: Required, StringLength(300)] string Reason);
public sealed record ProductResponse(Guid Id, string Sku, string Name,
    int Stock, int MinimumStock, long UnitPriceCents, bool LowStock);
public sealed record MovementResponse(Guid Id, Guid ProductId, MovementType Type,
    int Quantity, string Reason, DateTime CreatedAtUtc);
