using Entites.RealTime.OHLC;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IOHLCService
{
    Task<OHLC> GetOHLCAsync(string symbol);
}
