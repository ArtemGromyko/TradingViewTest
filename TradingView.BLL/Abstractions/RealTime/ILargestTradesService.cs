using Entites.RealTime.LargestTrade;

namespace TradingView.BLL.Abstractions.RealTime;

public interface ILargestTradesService
{
    Task<List<LargestTradeItem>> GetLargestTradesListAsync(string symbol);
}
