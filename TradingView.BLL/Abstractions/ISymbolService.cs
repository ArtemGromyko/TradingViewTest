using Entites;

namespace TradingView.BLL.Abstractions;

public interface ISymbolService
{
    Task<List<SymbolInfo>> GetSymbolsAsync();
}
