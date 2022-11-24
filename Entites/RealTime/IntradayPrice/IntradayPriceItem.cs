namespace Entites.RealTime.IntradayPrice;

public class IntradayPriceItem
{
    public string Date { get; set; }
    public string Minute { get; set; }
    public string Label { get; set; }
    public double? High { get; set; }
    public double? Low { get; set; }
    public double? Open { get; set; }
    public double? Close { get; set; }
    public double? Average { get; set; }
    public double? Volume { get; set; }
    public double? Notional { get; set; }
    public long? NumberOfTrades { get; set; }
}