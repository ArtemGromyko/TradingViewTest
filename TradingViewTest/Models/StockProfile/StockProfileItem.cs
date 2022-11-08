using Newtonsoft.Json;

namespace TradingViewTest.Models.StockProfile
{
    public class StockProfileItem
    {
        [JsonProperty("ceo-compensation")]
        public CEOCompensation CeoCompensation { get; set; }
        public Company Company { get; set; }
        public Logo Logo { get; set; }
        public List<string> Peers { get; set; }
        [JsonProperty("insider-transactions")]
        public List<InsiderTransactions> InsiderTransactions { get; set; }
        [JsonProperty("insider-roster")]
        public List<InsiderRoster> InsiderRoster { get; set; }
        [JsonProperty("insider-summary")]
        public List<InsiderSummary> InsiderSummary { get; set; }
    }
}
