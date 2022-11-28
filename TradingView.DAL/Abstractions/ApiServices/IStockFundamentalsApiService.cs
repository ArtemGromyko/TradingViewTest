using Entites;
using Entites.StockFundamentals;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface IStockFundamentalsApiService
{
    Task<(StockFundamentals, ResponseDto)> FetchStockFundamentalsAsync(string symbol);
}
