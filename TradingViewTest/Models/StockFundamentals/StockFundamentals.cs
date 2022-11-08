using Newtonsoft.Json;

namespace TradingViewTest.Models.StockFundamentals
{
    public class StockFundamentals : EntityBase
    {
        [JsonProperty("symbol-name")]
        public string SymbolName { get; set; }
        [JsonProperty("stock-fundamentals")]
        public StockFundamentalsItem StockFundamentalsItem { get; set; }
    }
}
