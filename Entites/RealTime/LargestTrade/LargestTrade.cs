namespace Entites.RealTime.LargestTrade;

public class LargestTrade : EntityBase
{
    public string Symbol { get; set; }
    public List<LargestTradeItem> Items { get; set; }
}
