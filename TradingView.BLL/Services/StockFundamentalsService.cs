using Entites.StockFundamentals;
using TradingView.BLL.Abstractions;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.BLL.Services;

public class StockFundamentalsService : IStockFundamentalsService
{
    private readonly IStockFundamentalsRepository _stockFundamentalsRepository;
    private readonly IStockFundamentalsApiService _stockFundamentalsApiService;

    public StockFundamentalsService(IStockFundamentalsRepository stockFundamentalsRepository, IStockFundamentalsApiService stockFundamentalsApiService)
    {
        _stockFundamentalsRepository = stockFundamentalsRepository;
        _stockFundamentalsApiService = stockFundamentalsApiService;
    }

    public async Task<StockFundamentals> GetStockFundamentalsAsync(string symbol)
    {
        var stockFundamentals = await _stockFundamentalsRepository.GetAsync(
            (s) => s.SymbolName.ToUpper() == symbol.ToUpper());

        //var stockFundamentals = await _stockFundamentalsApiService.FetchStockFundamentalsAsync(symbol);

        return stockFundamentals;
    }
}
