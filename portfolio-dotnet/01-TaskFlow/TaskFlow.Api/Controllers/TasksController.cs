using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Core.Contracts;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Services;
namespace TaskFlow.Api.Controllers;
[ApiController]
[Authorize]
[Route("api/tasks")]
public sealed class TasksController(TaskService tasks) : ControllerBase
{
    private Guid OwnerId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    [HttpGet]
    public async Task<ActionResult<PageResponse<TaskResponse>>> List(
        [FromQuery] WorkStatus? status, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20, CancellationToken ct = default) =>
        Ok(await tasks.ListAsync(OwnerId, status, page, pageSize, ct));
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponse>> Get(Guid id, CancellationToken ct) =>
        Ok(await tasks.GetAsync(OwnerId, id, ct));
    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest input, CancellationToken ct)
    {
        var created = await tasks.CreateAsync(OwnerId, input, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskResponse>> Update(Guid id, UpdateTaskRequest input, CancellationToken ct) =>
        Ok(await tasks.UpdateAsync(OwnerId, id, input, ct));
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<TaskResponse>> ChangeStatus(Guid id, ChangeStatusRequest input, CancellationToken ct) =>
        Ok(await tasks.ChangeStatusAsync(OwnerId, id, input.Status, ct));
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await tasks.DeleteAsync(OwnerId, id, ct);
        return NoContent();
    }
}
