using Entites;
using Entites.StockProfile;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface IStockProfileApiService
{
    Task<(StockProfile, ResponseDto)> FetchStockProfileAsync(SymbolInfo symbol);
}
