using FieldOpsAI.Core.Domain;
using FieldOpsAI.Infrastructure.AI;
using Xunit;
namespace FieldOpsAI.Tests;
public class TriageEngineTests
{
    [Fact]
    public async Task CaboCaido_EhClassificadoCriticoPelasRegras()
    {
        var result = await new RuleBasedTriageEngine().AnalyzeAsync(
            "Relato fictício: fio caído junto ao poste", CancellationToken.None);
        Assert.Equal(IncidentPriority.Critical, result.Priority);
        Assert.Equal(IncidentCategory.Electrical, result.Category);
        Assert.Equal("rules-demo", result.Source);
    }
    [Fact]
    public async Task FalhaDeInternet_EhClassificadaComoRede()
    {
        var result = await new RuleBasedTriageEngine().AnalyzeAsync(
            "Sem internet no roteador de teste", CancellationToken.None);
        Assert.Equal(IncidentCategory.Network, result.Category);
        Assert.Equal(IncidentPriority.Normal, result.Priority);
    }
}
