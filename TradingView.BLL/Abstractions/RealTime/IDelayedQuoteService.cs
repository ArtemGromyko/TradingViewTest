using Entites.RealTime;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IDelayedQuoteService
{
    Task<DelayedQuote> GetDelayedQuoteAsync(string symbol);
}
