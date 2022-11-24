using Entites.RealTime;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IHistoricalPricesService
{
    Task<List<HistoricalPriceItem>> GetHistoricalPricesListAsync(string symbol);
}
