using Entites.RealTime;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IPreviousDayPriceService
{
    Task<PreviousDayPrice> GetPreviousDayPriceAsync(string symbol);
}
