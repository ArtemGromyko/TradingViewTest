namespace TradingViewTest.Models.StockFundamentals.Financials
{
    public class FinancialsEntity
    {
        public string Symbol { get; set; }
        public List<FinancialsItem> Financials { get; set; }
    }
}
