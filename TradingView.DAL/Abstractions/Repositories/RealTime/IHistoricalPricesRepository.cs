using Entites.RealTime;

namespace TradingView.DAL.Abstractions.Repositories.RealTime;

public interface IHistoricalPricesRepository : IRepositoryBase<HistoricalPrice>
{
    Task UpdateAsync(HistoricalPrice historicalPrice);
}
