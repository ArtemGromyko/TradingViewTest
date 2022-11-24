namespace TradingView.BLL.Abstractions.RealTime;

public interface IPriceOnlyService
{
    Task<double> GetPriceOnlyAsync(string symbol);
}
