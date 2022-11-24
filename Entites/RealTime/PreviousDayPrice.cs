using MongoDB.Bson.Serialization.Attributes;

namespace Entites.RealTime;

[BsonNoId]
public class PreviousDayPrice : EntityBase
{
    public double? Close { get; set; }
    public double? High { get; set; }
    public double? Low { get; set; }
    public double? Open { get; set; }
    public string PriceDate { get; set; }
    public string Symbol { get; set; }
    public double? Volume { get; set; }
    [BsonElement("HistoricalPriceId")]
    public string Id { get; set; }
    public string Key { get; set; }
    public string Subkey { get; set; }
    public string Date { get; set; }
    public double? Updated { get; set; }
    public double? ChangeOverTime { get; set; }
    public double? MarketChangeOverTime { get; set; }
    public double? UOpen { get; set; }
    public double? UClose { get; set; }
    public double? UHigh { get; set; }
    public double? ULow { get; set; }
    public double? UVolume { get; set; }
    public double? FOpen { get; set; }
    public double? FClose { get; set; }
    public double? FHigh { get; set; }
    public double? FLow { get; set; }
    public double? FVolume { get; set; }
    public string Label { get; set; }
    public double? Change { get; set; }
    public double? ChangePercent { get; set; }
}
