using FieldOpsAI.Core.Exceptions;
namespace FieldOpsAI.Core.Domain;
public enum IncidentPriority { Normal, High, Critical }
public enum IncidentCategory { Electrical, Equipment, Network, Other }
public enum IncidentStatus { Open, Triaged, Reviewed }
public sealed record TriageDecision(IncidentPriority Priority, IncidentCategory Category,
    string Summary, string Guidance, string Source);
public sealed class Incident
{
    private Incident() { }
    public Incident(string location, string description)
    {
        if (string.IsNullOrWhiteSpace(location) || location.Length > 160)
            throw new BusinessRuleException("Local obrigatório (até 160 caracteres).");
        if (string.IsNullOrWhiteSpace(description) || description.Length > 2000)
            throw new BusinessRuleException("Descrição obrigatória (até 2.000 caracteres).");
        Id = Guid.NewGuid();
        Location = location.Trim();
        Description = description.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }
    public Guid Id { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public IncidentStatus Status { get; private set; } = IncidentStatus.Open;
    public IncidentPriority? Priority { get; private set; }
    public IncidentCategory? Category { get; private set; }
    public string? Summary { get; private set; }
    public string? Guidance { get; private set; }
    public string? TriageSource { get; private set; }
    public void ApplyTriage(TriageDecision result)
    {
        if (Status == IncidentStatus.Reviewed)
            throw new BusinessRuleException("Ocorrência já revisada não pode ser reclassificada.");
        Priority = result.Priority;
        Category = result.Category;
        Summary = result.Summary;
        Guidance = result.Guidance;
        TriageSource = result.Source;
        Status = IncidentStatus.Triaged;
    }
    public void MarkReviewed()
    {
        if (Status != IncidentStatus.Triaged)
            throw new BusinessRuleException("Somente ocorrências triadas podem ser revisadas.");
        Status = IncidentStatus.Reviewed;
    }
}
