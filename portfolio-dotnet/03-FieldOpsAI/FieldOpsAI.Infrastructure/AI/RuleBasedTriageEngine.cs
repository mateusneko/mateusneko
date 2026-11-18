using FieldOpsAI.Core.Domain;
using FieldOpsAI.Core.Ports;
namespace FieldOpsAI.Infrastructure.AI;
// Classificador determinístico demonstrativo; NÃO é um modelo de IA.
public sealed class RuleBasedTriageEngine : ITriageEngine
{
    public Task<TriageDecision> AnalyzeAsync(string description, CancellationToken ct)
    {
        var text = description.ToLowerInvariant();
        var critical = Has(text, "choque", "incêndio", "incendio", "arco elétrico", "arco eletrico",
            "cabo caído", "cabo caido", "fio caído", "fio caido", "eletrocussão");
        var electrical = critical || Has(text, "poste", "transformador", "tensão", "tensao", "rede elétrica", "rede eletrica", "disjuntor");
        var network = Has(text, "internet", "conexão", "conexao", "roteador", "wi-fi", "servidor");
        var equipment = Has(text, "máquina", "maquina", "ferramenta", "motor", "equipamento");
        var category = electrical ? IncidentCategory.Electrical : network ? IncidentCategory.Network :
            equipment ? IncidentCategory.Equipment : IncidentCategory.Other;
        var priority = critical ? IncidentPriority.Critical : electrical ? IncidentPriority.High : IncidentPriority.Normal;
        var guidance = critical
            ? "Não se aproxime da área de risco. Acione imediatamente a equipe responsável e os protocolos de emergência da organização."
            : "Encaminhe para avaliação de profissional habilitado e siga os procedimentos internos vigentes.";
        var summary = description.Trim().Length > 140 ? description.Trim()[..140] : description.Trim();
        return Task.FromResult(new TriageDecision(priority, category, summary, guidance, "rules-demo"));
    }
    private static bool Has(string text, params string[] keywords) => keywords.Any(text.Contains);
}
