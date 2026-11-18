using FieldOpsAI.Core.Contracts;
using FieldOpsAI.Core.Domain;
using FieldOpsAI.Core.Exceptions;
using FieldOpsAI.Core.Ports;
namespace FieldOpsAI.Core.Services;
public sealed class IncidentService(IIncidentRepository repository, ITriageEngine engine)
{
    private static IncidentResponse Map(Incident incident) => new(
        incident.Id, incident.Location, incident.Description, incident.CreatedAtUtc, incident.Status,
        incident.Priority, incident.Category, incident.Summary, incident.Guidance, incident.TriageSource);
    public async Task<IncidentResponse> CreateAsync(CreateIncidentRequest request, CancellationToken ct)
    {
        var incident = new Incident(request.Location, request.Description);
        await repository.AddAsync(incident, ct);
        return Map(incident);
    }
    public async Task<IncidentResponse> GetAsync(Guid id, CancellationToken ct) =>
        Map(await repository.GetAsync(id, ct) ?? throw new NotFoundException("Ocorrência não encontrada."));
    public async Task<IReadOnlyList<IncidentResponse>> ListAsync(CancellationToken ct) =>
        (await repository.ListAsync(ct)).Select(Map).ToList();
    public async Task<IncidentResponse> TriageAsync(Guid id, CancellationToken ct)
    {
        var incident = await repository.GetAsync(id, ct)
            ?? throw new NotFoundException("Ocorrência não encontrada.");
        if (incident.Status == IncidentStatus.Reviewed)
            throw new BusinessRuleException("Ocorrência já revisada não pode ser reclassificada.");
        var decision = await engine.AnalyzeAsync(incident.Description, ct);
        incident.ApplyTriage(decision);
        await repository.SaveAsync(ct);
        return Map(incident);
    }
    public async Task<IncidentResponse> ReviewAsync(Guid id, CancellationToken ct)
    {
        var incident = await repository.GetAsync(id, ct)
            ?? throw new NotFoundException("Ocorrência não encontrada.");
        incident.MarkReviewed();
        await repository.SaveAsync(ct);
        return Map(incident);
    }
}
