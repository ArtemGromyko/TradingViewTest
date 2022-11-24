namespace Entites.RealTime.LargestTrade;

public class LargestTradeItem
{
    public double? Price { get; set; }
    public long? Size { get; set; }
    public long? Time { get; set; }
    public string TimeLabel { get; set; }
    public string VenueName { get; set; }
    public string Venue { get; set; }
}
