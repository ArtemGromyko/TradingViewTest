using Entites;
using Entites.StockFundamentals.FinancialsAsReported;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface IFinancialsAsReportedApiService
{
    Task<(FinancialsAsReported, ResponseDto)> FetchFinancialsAsReportedAsync(string symbol);
}
