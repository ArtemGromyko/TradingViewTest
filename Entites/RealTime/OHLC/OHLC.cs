namespace Entites.RealTime.OHLC;

public class OHLC : EntityBase
{
    public Open Open { get; set; }
    public Close Close { get; set; }
    public double? High { get; set; }
    public double? Low { get; set; }
    public decimal? Volume { get; set; }
    public string Symbol { get; set; }
}