using System.Text.Json;
using FieldOpsAI.Core.Domain;
using FieldOpsAI.Core.Ports;
namespace FieldOpsAI.Infrastructure.AI;
// Se a API remota falhar, a aplicação continua com classificação explícita por regras.
public sealed class FallbackTriageEngine(ITriageEngine primary, ITriageEngine backup) : ITriageEngine
{
    public async Task<TriageDecision> AnalyzeAsync(string description, CancellationToken ct)
    {
        try { return await primary.AnalyzeAsync(description, ct); }
        catch (HttpRequestException) { }
        catch (JsonException) { }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested) { }
        var result = await backup.AnalyzeAsync(description, ct);
        return result with { Source = "rules-fallback" };
    }
}
