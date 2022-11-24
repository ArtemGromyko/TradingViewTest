using Entites.RealTime.IntradayPrice;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IIntradayPricesService
{
    Task<List<IntradayPriceItem>> GetIntradayPricesListAsync(string symbol);
}
