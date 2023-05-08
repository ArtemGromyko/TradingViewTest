using Entites.RealTime.Book;
using Entites.RealTime.IntradayPrice;
using Entites.RealTime.LargestTrade;
using Entites.RealTime.VolumeByVenue;
using Newtonsoft.Json;
using Ohlc = Entites.RealTime.OHLC.OHLC;

namespace Entites.RealTime
{
    public class RealTimeItem
    {
        public List<LargestTradeItem> LargestTrades { get; set; }

        public Ohlc OHLC { get; set; }

        public DelayedQuote DelayedQuote { get; set; }

        [JsonProperty("volume-by-venue")]
        public List<VolumeByVenueItem> VolumeByVenue { get; set; }

        public Quote Quote { get; set; }

        public List<HistoricalPriceItem> Chart { get; set; }

        public decimal Price { get; set; }

        public BookItem Book { get; set; }

        public List<IntradayPriceItem> IntradayPrices { get; set; }

        public PreviousDayPrice Previous { get; set; }
    }
}
