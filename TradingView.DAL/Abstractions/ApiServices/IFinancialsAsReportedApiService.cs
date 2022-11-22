using Entites.StockFundamentals.FinancialsAsReported;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface IFinancialsAsReportedApiService
{
    Task<FinancialsAsReported> FetchFinancialsAsReportedAsync(string symbol);
}
