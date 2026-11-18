using Microsoft.AspNetCore.Mvc;
using StockPilot.Core.Contracts;
using StockPilot.Core.Services;
namespace StockPilot.Api.Controllers;
[ApiController]
[Route("api/products")]
public sealed class ProductsController(InventoryService inventory) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest input, CancellationToken ct)
    {
        var item = await inventory.CreateProductAsync(input, ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> List(CancellationToken ct) =>
        Ok(await inventory.ListAsync(false, ct));
    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> LowStock(CancellationToken ct) =>
        Ok(await inventory.ListAsync(true, ct));
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Get(Guid id, CancellationToken ct) =>
        Ok(await inventory.GetAsync(id, ct));
    [HttpPost("{id:guid}/movements")]
    public async Task<ActionResult<MovementResponse>> Move(Guid id, CreateMovementRequest input, CancellationToken ct) =>
        Ok(await inventory.MoveAsync(id, input, ct));
    [HttpGet("{id:guid}/movements")]
    public async Task<ActionResult<IReadOnlyList<MovementResponse>>> History(Guid id, CancellationToken ct) =>
        Ok(await inventory.HistoryAsync(id, ct));
}
