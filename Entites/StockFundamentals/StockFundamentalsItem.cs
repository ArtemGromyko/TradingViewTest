using Entites.StockFundamentals.BalanceSheet;
using Entites.StockFundamentals.CashFlow;
using Entites.StockFundamentals.Financials;
using Entites.StockFundamentals.IncomeStatement;
using Newtonsoft.Json;

namespace Entites.StockFundamentals;

public class StockFundamentalsItem
{
    [JsonProperty("balance-sheet")]
    public BalanceSheetEntity BalanceSheet { get; set; }
    public FinancialsEntity Financials { get; set; }
    public List<Split> Splits { get; set; }
    public List<Dividend> Dividends { get; set; }
    public IncomeEntity Income { get; set; }
    [JsonProperty("cash-flow")]
    public CashFlowEntity CashFlow { get; set; }
}
