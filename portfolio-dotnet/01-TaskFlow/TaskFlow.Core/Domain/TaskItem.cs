namespace TaskFlow.Core.Domain;
public enum WorkStatus { Pending, InProgress, Done }
public sealed class TaskItem
{
    private TaskItem() { } // EF Core
    public TaskItem(Guid ownerId, string title, string? description, DateTime? dueAtUtc)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        CreatedAtUtc = DateTime.UtcNow;
        Update(title, description, dueAtUtc);
    }
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public WorkStatus Status { get; private set; } = WorkStatus.Pending;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? DueAtUtc { get; private set; }
    public void Update(string title, string? description, DateTime? dueAtUtc)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > 120)
            throw new ArgumentException("Título obrigatório (até 120 caracteres).");
        Title = title.Trim();
        Description = description?.Trim();
        DueAtUtc = dueAtUtc;
    }
    public void ChangeStatus(WorkStatus status)
    {
        if (!Enum.IsDefined(status)) throw new ArgumentException("Status inválido.");
        Status = status;
    }
}
