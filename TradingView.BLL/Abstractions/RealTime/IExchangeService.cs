using Entites.RealTime;

namespace TradingView.BLL.Contracts;

public interface IExchangeService
{
    Task<List<ExchangeInfo>> GetExchangesAsync();
}
