using Entites.StockFundamentals;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface IStockFundamentalsApiService
{
    Task<StockFundamentals> FetchStockFundamentalsAsync(string symbol);
}
