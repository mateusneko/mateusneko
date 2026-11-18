using FieldOpsAI.Core.Domain;
using FieldOpsAI.Core.Exceptions;
using Xunit;
namespace FieldOpsAI.Tests;
public class IncidentDomainTests
{
    [Fact]
    public void Triagem_DeveSerRevisadaPorPessoa()
    {
        var incident = new Incident("Área simulada", "Equipamento parou de responder");
        incident.ApplyTriage(new TriageDecision(IncidentPriority.Normal,
            IncidentCategory.Equipment, "Falha de equipamento", "Revisar", "rules-demo"));
        Assert.Equal(IncidentStatus.Triaged, incident.Status);
        incident.MarkReviewed();
        Assert.Equal(IncidentStatus.Reviewed, incident.Status);
    }
    [Fact]
    public void NaoPodeRevisarAntesDaTriagem()
    {
        var incident = new Incident("Lab", "Teste");
        Assert.Throws<BusinessRuleException>(() => incident.MarkReviewed());
    }
}
