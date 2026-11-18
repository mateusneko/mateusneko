using TaskFlow.Core.Domain;
using Xunit;
namespace TaskFlow.Tests;
public class TaskDomainTests
{
    [Fact]
    public void NovaTarefa_ComecaPendente()
    {
        var task = new TaskItem(Guid.NewGuid(), "Estudar LINQ", null, null);
        Assert.Equal(WorkStatus.Pending, task.Status);
    }
    [Fact]
    public void AtualizarTituloVazio_DeveFalhar()
    {
        var task = new TaskItem(Guid.NewGuid(), "Valido", null, null);
        Assert.Throws<ArgumentException>(() => task.Update("  ", null, null));
    }
    [Fact]
    public void AlterarStatus_MantemNovoStatus()
    {
        var task = new TaskItem(Guid.NewGuid(), "Testes", null, null);
        task.ChangeStatus(WorkStatus.Done);
        Assert.Equal(WorkStatus.Done, task.Status);
    }
}
