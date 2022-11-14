namespace Entites.StockFundamentals.CashFlow;

public class CashFlowEntity
{
    public string Symbol { get; set; }
    public List<CashFlowItem> CashFlow { get; set; }
}
