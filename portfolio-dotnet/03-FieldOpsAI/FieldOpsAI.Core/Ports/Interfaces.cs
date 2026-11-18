using FieldOpsAI.Core.Domain;
namespace FieldOpsAI.Core.Ports;
public interface IIncidentRepository
{
    Task<Incident?> GetAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Incident>> ListAsync(CancellationToken ct);
    Task AddAsync(Incident incident, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);
}
public interface ITriageEngine
{
    Task<TriageDecision> AnalyzeAsync(string description, CancellationToken ct);
}
