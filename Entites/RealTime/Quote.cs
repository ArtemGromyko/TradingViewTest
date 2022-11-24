using MongoDB.Bson.Serialization.Attributes;

namespace Entites.RealTime;


[BsonNoId]
public class Quote : EntityBase
{
    public long? AvgTotalVolume { get; set; }
    public string CalculationPrice { get; set; }
    public double? Change { get; set; }
    public double? ChangePercent { get; set; }
    public double? Close { get; set; }
    public string CloseSource { get; set; }
    public decimal? CloseTime { get; set; }
    public string CompanyName { get; set; }
    public string Currency { get; set; }
    public decimal? DelayedPrice { get; set; }
    public decimal? DelayedPriceTime { get; set; }
    public decimal? ExtendedChange { get; set; }
    public decimal? ExtendedChangePercent { get; set; }
    public decimal? ExtendedPrice { get; set; }
    public decimal? ExtendedPriceTime { get; set; }
    public double? High { get; set; }
    public string HighSource { get; set; }
    public decimal? HighTime { get; set; }
    public decimal? IexAskPrice { get; set; }
    public long? IexAskSize { get; set; }
    public decimal? IexBidPrice { get; set; }
    public long? IexBidSize { get; set; }
    public double? IexClose { get; set; }
    public long? IexCloseTime { get; set; }
    public long? IexLastUpdated { get; set; }
    public double? IexMarketPercent { get; set; }
    public double? IexOpen { get; set; }
    public long? IexOpenTime { get; set; }
    public double? IexRealtimePrice { get; set; }
    public long? IexRealtimeSize { get; set; }
    public long? IexVolume { get; set; }
    public long? LastTradeTime { get; set; }
    public double? LatestPrice { get; set; }
    public string LatestSource { get; set; }
    public string LatestTime { get; set; }
    public long? LatestUpdate { get; set; }
    public decimal? LatestVolume { get; set; }
    public double? Low { get; set; }
    public string LowSource { get; set; }
    public long? LowTime { get; set; }
    public long? MarketCap { get; set; }
    public decimal? OddLotDelayedPrice { get; set; }
    public decimal? OddLotDelayedPriceTime { get; set; }
    public double? Open { get; set; }
    public decimal? OpenTime { get; set; }
    public string OpenSource { get; set; }
    public double? PeRatio { get; set; }
    public double? PreviousClose { get; set; }
    public long? PreviousVolume { get; set; }
    public string PrimaryExchange { get; set; }
    public string Symbol { get; set; }
    public decimal? Volume { get; set; }
    public double? Week52High { get; set; }
    public double? Week52Low { get; set; }
    public double? YtdChange { get; set; }
    public bool? IsUSMarketOpen { get; set; }
}
