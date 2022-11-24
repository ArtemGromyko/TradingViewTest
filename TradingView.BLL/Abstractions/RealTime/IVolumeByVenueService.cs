using Entites.RealTime.VolumeByVenue;

namespace TradingView.BLL.Abstractions.RealTime;

public interface IVolumeByVenueService
{
    Task<List<VolumeByVenueItem>> GetVolumesByVenueAsync(string symbol);
}
