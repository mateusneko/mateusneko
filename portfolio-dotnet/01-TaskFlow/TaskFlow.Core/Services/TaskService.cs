using TaskFlow.Core.Contracts;
using TaskFlow.Core.Domain;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Ports;
namespace TaskFlow.Core.Services;
public sealed class TaskService(ITaskRepository tasks)
{
    private static TaskResponse ToResponse(TaskItem task) =>
        new(task.Id, task.Title, task.Description, task.Status, task.CreatedAtUtc, task.DueAtUtc);
    public async Task<TaskResponse> CreateAsync(Guid ownerId, CreateTaskRequest input, CancellationToken ct)
    {
        var item = new TaskItem(ownerId, input.Title, input.Description, input.DueAtUtc);
        await tasks.AddAsync(item, ct);
        return ToResponse(item);
    }
    public async Task<TaskResponse> GetAsync(Guid ownerId, Guid id, CancellationToken ct)
    {
        var item = await tasks.GetOwnedAsync(id, ownerId, ct)
            ?? throw new NotFoundException("Tarefa não encontrada.");
        return ToResponse(item);
    }
    public async Task<PageResponse<TaskResponse>> ListAsync(Guid ownerId, WorkStatus? status,
        int page, int pageSize, CancellationToken ct)
    {
        if (page < 1 || pageSize is < 1 or > 100)
            throw new BusinessRuleException("page deve ser >= 1 e pageSize entre 1 e 100.");
        var (items, total) = await tasks.ListAsync(ownerId, status, page, pageSize, ct);
        return new PageResponse<TaskResponse>(items.Select(ToResponse).ToList(), page, pageSize, total);
    }
    public async Task<TaskResponse> UpdateAsync(Guid ownerId, Guid id, UpdateTaskRequest input, CancellationToken ct)
    {
        var item = await tasks.GetOwnedAsync(id, ownerId, ct)
            ?? throw new NotFoundException("Tarefa não encontrada.");
        item.Update(input.Title, input.Description, input.DueAtUtc);
        await tasks.SaveAsync(ct);
        return ToResponse(item);
    }
    public async Task<TaskResponse> ChangeStatusAsync(Guid ownerId, Guid id, WorkStatus status, CancellationToken ct)
    {
        var item = await tasks.GetOwnedAsync(id, ownerId, ct)
            ?? throw new NotFoundException("Tarefa não encontrada.");
        item.ChangeStatus(status);
        await tasks.SaveAsync(ct);
        return ToResponse(item);
    }
    public async Task DeleteAsync(Guid ownerId, Guid id, CancellationToken ct)
    {
        var item = await tasks.GetOwnedAsync(id, ownerId, ct)
            ?? throw new NotFoundException("Tarefa não encontrada.");
        tasks.Delete(item);
        await tasks.SaveAsync(ct);
    }
}
