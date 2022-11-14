using Entites.StockFundamentals.BalanceSheet;
using Entites.StockFundamentals.CashFlow;
using Entites.StockFundamentals.Financials;
using Entites.StockFundamentals.IncomeStatement;
using Newtonsoft.Json;

namespace Entites.StockFundamentals;

public class StockFundamentalsItem : EntityBase
{
    public FinancialsEntity Financials { get; set; }
    public List<Split> Splits { get; set; }
    [JsonProperty("balance-sheet")]
    public BalanceSheetEntity BalanceSheet { get; set; }
    [JsonProperty("cash-flow")]
    public CashFlowEntity CashFlow { get; set; }
    public IncomeEntity Income { get; set; }
    public List<Dividend> Dividends { get; set; }
}
