using Newtonsoft.Json;

namespace TradingViewTest.Models.StockProfile;

public class StockProfile : EntityBase
{
    [JsonProperty("symbol-name")]
    public string SymbolName { get; set; }
    [JsonProperty("stock-profile")]
    public StockProfileItem StockProfileItem { get; set; }
}
