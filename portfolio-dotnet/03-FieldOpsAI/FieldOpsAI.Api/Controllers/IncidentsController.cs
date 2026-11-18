using FieldOpsAI.Core.Contracts;
using FieldOpsAI.Core.Services;
using Microsoft.AspNetCore.Mvc;
namespace FieldOpsAI.Api.Controllers;
[ApiController]
[Route("api/incidents")]
public sealed class IncidentsController(IncidentService incidents) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<IncidentResponse>> Create(CreateIncidentRequest input, CancellationToken ct)
    {
        var item = await incidents.CreateAsync(input, ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<IncidentResponse>>> List(CancellationToken ct) =>
        Ok(await incidents.ListAsync(ct));
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IncidentResponse>> Get(Guid id, CancellationToken ct) =>
        Ok(await incidents.GetAsync(id, ct));
    [HttpPost("{id:guid}/triage")]
    public async Task<ActionResult<IncidentResponse>> Triage(Guid id, CancellationToken ct) =>
        Ok(await incidents.TriageAsync(id, ct));
    [HttpPost("{id:guid}/review")]
    public async Task<ActionResult<IncidentResponse>> Review(Guid id, CancellationToken ct) =>
        Ok(await incidents.ReviewAsync(id, ct));
}
