using FieldOpsAI.Core.Domain;
using FieldOpsAI.Core.Ports;
using FieldOpsAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace FieldOpsAI.Infrastructure.Repositories;
public sealed class IncidentRepository(IncidentsDbContext db) : IIncidentRepository
{
    public Task<Incident?> GetAsync(Guid id, CancellationToken ct) =>
        db.Incidents.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<Incident>> ListAsync(CancellationToken ct) =>
        await db.Incidents.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).Take(100).ToListAsync(ct);
    public async Task AddAsync(Incident incident, CancellationToken ct)
    {
        db.Incidents.Add(incident);
        await db.SaveChangesAsync(ct);
    }
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
