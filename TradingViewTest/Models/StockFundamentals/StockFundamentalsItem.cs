using Newtonsoft.Json;
using TradingViewTest.Models.StockFundamentals.BalanceSheet;
using TradingViewTest.Models.StockFundamentals.CashFlow;
using TradingViewTest.Models.StockFundamentals.Financials;
using TradingViewTest.Models.StockFundamentals.IncomeStatement;

namespace TradingViewTest.Models.StockFundamentals
{
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
}
