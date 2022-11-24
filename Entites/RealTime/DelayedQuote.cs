namespace Entites.RealTime;

public class DelayedQuote : EntityBase
{
    public string Symbol { get; set; }
    public double? DelayedPrice { get; set; }
    public double? High { get; set; }
    public double? Low { get; set; }
    public long? DelayedSize { get; set; }
    public long? DelayedPriceTime { get; set; }
    public long? TotalVolume { get; set; }
    public long? ProcessedTime { get; set; }
}