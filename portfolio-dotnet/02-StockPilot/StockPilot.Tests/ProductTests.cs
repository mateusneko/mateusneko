using StockPilot.Core.Domain;
using StockPilot.Core.Exceptions;
using Xunit;
namespace StockPilot.Tests;
public class ProductTests
{
    [Fact]
    public void EntradaESaida_AtualizamSaldo()
    {
        var product = new Product("m-001", "Cabo 10 mm²", 2, 500);
        product.ApplyMovement(MovementType.Entry, 10);
        product.ApplyMovement(MovementType.Exit, 3);
        Assert.Equal(7, product.Stock);
        Assert.False(product.IsLowStock);
    }
    [Fact]
    public void SaidaMaiorQueEstoque_DeveSerRejeitada()
    {
        var product = new Product("m-002", "Conector", 5, 100);
        Assert.Throws<BusinessRuleException>(() => product.ApplyMovement(MovementType.Exit, 1));
        Assert.Equal(0, product.Stock);
    }
    [Fact]
    public void MovimentacaoAlteraVersaoDeConcorrencia()
    {
        var product = new Product("m-003", "Isolador", 1, 210);
        var oldVersion = product.Version;
        product.ApplyMovement(MovementType.Entry, 1);
        Assert.NotEqual(oldVersion, product.Version);
    }
    [Fact]
    public void ProdutoNaoPodeTerPrecoNegativo() =>
        Assert.Throws<BusinessRuleException>(() => new Product("x", "Teste", 0, -1));
}
