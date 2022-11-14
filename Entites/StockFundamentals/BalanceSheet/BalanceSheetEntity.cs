namespace Entites.StockFundamentals.BalanceSheet;

public class BalanceSheetEntity
{
    public string Symbol { get; set; }
    public List<BalanceSheetItem> BalanceSheet { get; set; }
}
