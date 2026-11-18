using FieldOpsAI.Core.Domain;
using FieldOpsAI.Infrastructure.Data;
using FieldOpsAI.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace FieldOpsAI.Tests;
public class IncidentRepositoryTests
{
    [Fact]
    public async Task SalvaEConsultaTriagemNoSqliteEmMemoria()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<IncidentsDbContext>().UseSqlite(connection).Options;
        await using var db = new IncidentsDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var repo = new IncidentRepository(db);
        var incident = new Incident("Local fictício", "Evento simulado de teste");
        await repo.AddAsync(incident, CancellationToken.None);
        incident.ApplyTriage(new TriageDecision(IncidentPriority.Normal, IncidentCategory.Other, "Teste", "Revisão", "rules-demo"));
        await repo.SaveAsync(CancellationToken.None);
        db.ChangeTracker.Clear();
        var loaded = await repo.GetAsync(incident.Id, CancellationToken.None);
        Assert.Equal(IncidentStatus.Triaged, loaded?.Status);
        Assert.Equal("rules-demo", loaded?.TriageSource);
    }
}
