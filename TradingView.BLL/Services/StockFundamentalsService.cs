using Entites.StockFundamentals;
using TradingView.BLL.Abstractions;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.BLL.Services;

public class StockFundamentalsService : IStockFundamentalsService
{
    private readonly IStockFundamentalsRepository _stockFundamentalsRepository;

    public StockFundamentalsService(IStockFundamentalsRepository stockFundamentalsRepository)
    {
        _stockFundamentalsRepository = stockFundamentalsRepository;
    }

    public async Task<StockFundamentals> GetStockFundamentalsAsync(string symbol)
    {
        var stockFundamentals = await _stockFundamentalsRepository.GetAsync(
            (s) => s.SymbolName.ToUpper() == symbol.ToUpper());

        return stockFundamentals;
    }
}
