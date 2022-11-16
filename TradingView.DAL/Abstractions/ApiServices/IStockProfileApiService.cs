using Entites.StockProfile;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface IStockProfileApiService
{
    Task<StockProfile> FetchStockProfileAsync(string symbol);
}
