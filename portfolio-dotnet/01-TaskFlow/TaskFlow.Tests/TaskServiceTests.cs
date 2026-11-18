using TaskFlow.Core.Contracts;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Ports;
using TaskFlow.Core.Services;
using Xunit;
namespace TaskFlow.Tests;
public class TaskServiceTests
{
    [Fact]
    public async Task UsuarioNaoPodeAcessarTarefaAlheia()
    {
        var owner = Guid.NewGuid();
        var other = Guid.NewGuid();
        var repo = new MemoryRepo();
        var item = new TaskItem(owner, "Privada", null, null);
        await repo.AddAsync(item, CancellationToken.None);
        var service = new TaskService(repo);
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetAsync(other, item.Id, CancellationToken.None));
    }
    private sealed class MemoryRepo : ITaskRepository
    {
        private readonly List<TaskItem> _items = [];
        public Task AddAsync(TaskItem task, CancellationToken ct) { _items.Add(task); return Task.CompletedTask; }
        public Task<TaskItem?> GetOwnedAsync(Guid id, Guid ownerId, CancellationToken ct) =>
            Task.FromResult(_items.FirstOrDefault(x => x.Id == id && x.OwnerId == ownerId));
        public Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(Guid id, WorkStatus? status,
            int page, int pageSize, CancellationToken ct)
        {
            var all = _items.Where(x => x.OwnerId == id && (status is null || x.Status == status)).ToList();
            return Task.FromResult(((IReadOnlyList<TaskItem>)all.Skip((page - 1) * pageSize).Take(pageSize).ToList(), all.Count));
        }
        public Task SaveAsync(CancellationToken ct) => Task.CompletedTask;
        public void Delete(TaskItem item) => _items.Remove(item);
    }
}
