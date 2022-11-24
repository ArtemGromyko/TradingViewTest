using Entites.RealTime;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IQuotesService
{
    Task<Quote> GetQuoteAsync(string symbol);
}
