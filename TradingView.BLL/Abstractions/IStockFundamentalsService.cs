using Entites.StockFundamentals;

namespace TradingView.BLL.Abstractions;

public interface IStockFundamentalsService
{
    Task<StockFundamentals> GetStockFundamentalsAsync(string symbol);
}
