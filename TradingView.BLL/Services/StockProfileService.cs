using Entites.StockProfile;
using TradingView.BLL.Abstractions;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.BLL.Services;

public class StockProfileService : IStockProfileService
{
    private readonly IStockProfileRepository _stockProfileRepository;

    public StockProfileService(IStockProfileRepository stockProfileRepository)
    {
        _stockProfileRepository = stockProfileRepository;
    }

    public async Task<StockProfile> GetStockProfileAsync(string symbol)
    {
        var stockProfile = await _stockProfileRepository.GetAsync(
            (s) => s.SymbolName.ToUpper() == symbol.ToUpper());

        return stockProfile;
    }
}
