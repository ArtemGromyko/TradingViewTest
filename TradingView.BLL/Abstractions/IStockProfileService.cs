using Entites.StockProfile;

namespace TradingView.BLL.Abstractions;

public interface IStockProfileService
{
    Task<StockProfile> GetStockProfileAsync(string symbol);
}
