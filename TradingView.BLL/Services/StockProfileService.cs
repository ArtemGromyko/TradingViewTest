using Entites.Exceptions;
using Entites.StockProfile;
using TradingView.BLL.Abstractions;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.BLL.Services;

public class StockProfileService : IStockProfileService
{
    private readonly IStockProfileRepository _stockProfileRepository;
    private readonly IStockProfileApiService _stockProfileApiService;

    public StockProfileService(IStockProfileRepository stockProfileRepository, IStockProfileApiService stockProfileApiService)
    {
        _stockProfileRepository = stockProfileRepository;
        _stockProfileApiService = stockProfileApiService;
    }

    public async Task<StockProfile> GetStockProfileAsync(string symbol)
    {
        var stockProfile = await _stockProfileRepository.GetAsync(
            (s) => s.SymbolName.ToUpper() == symbol.ToUpper());

        if (stockProfile is null)
        {
            throw new NotFoundException("Symbol not found");
        }

        return stockProfile;
    }
}
