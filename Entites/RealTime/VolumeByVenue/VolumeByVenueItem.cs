namespace Entites.RealTime.VolumeByVenue;

public class VolumeByVenueItem
{
    public long? Volume { get; set; }
    public string Venue { get; set; }
    public string VenueName { get; set; }
    public decimal? MarketPercent { get; set; }
    public decimal? AvgMarketPercent { get; set; }
    public string Date { get; set; }
}
