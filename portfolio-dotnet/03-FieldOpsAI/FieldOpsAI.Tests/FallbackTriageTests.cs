using FieldOpsAI.Core.Domain;
using FieldOpsAI.Core.Ports;
using FieldOpsAI.Infrastructure.AI;
using Xunit;
namespace FieldOpsAI.Tests;
public class FallbackTriageTests
{
    [Fact]
    public async Task FalhaRemota_DeveAcionarRegrasSemOcultarOrigem()
    {
        var engine = new FallbackTriageEngine(new FailingRemote(), new RuleBasedTriageEngine());
        var result = await engine.AnalyzeAsync("Equipamento com falha", CancellationToken.None);
        Assert.Equal("rules-fallback", result.Source);
        Assert.Equal(IncidentCategory.Equipment, result.Category);
    }
    private sealed class FailingRemote : ITriageEngine
    {
        public Task<TriageDecision> AnalyzeAsync(string description, CancellationToken ct) =>
            throw new HttpRequestException("Serviço indisponível para teste.");
    }
}
