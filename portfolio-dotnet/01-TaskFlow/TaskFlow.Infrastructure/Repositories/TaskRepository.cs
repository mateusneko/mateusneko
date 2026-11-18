using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Ports;
using TaskFlow.Infrastructure.Data;
namespace TaskFlow.Infrastructure.Repositories;
public sealed class TaskRepository(TaskFlowDbContext db) : ITaskRepository
{
    public async Task AddAsync(TaskItem item, CancellationToken ct)
    {
        db.Tasks.Add(item);
        await db.SaveChangesAsync(ct);
    }
    public Task<TaskItem?> GetOwnedAsync(Guid id, Guid ownerId, CancellationToken ct) =>
        db.Tasks.FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == ownerId, ct);
    public async Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(
        Guid ownerId, WorkStatus? status, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Tasks.AsNoTracking().Where(x => x.OwnerId == ownerId);
        if (status is not null) query = query.Where(x => x.Status == status.Value);
        var total = await query.CountAsync(ct);
        var items = await query.OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
    public void Delete(TaskItem item) => db.Tasks.Remove(item);
}
