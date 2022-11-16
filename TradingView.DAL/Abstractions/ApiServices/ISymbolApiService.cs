using Entites;

namespace TradingView.DAL.Abstractions.ApiServices;

public interface ISymbolApiService
{
    Task<List<SymbolInfo>> FetchSymbolsAsync();
}
