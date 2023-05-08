using Entites;

namespace TradingView.BLL.Abstractions.RealTime
{
    public interface IRealTimeService
    {
        Task<RealTimeDto> GetRealTimeDataAsync(string symbol);
    }
}
