namespace Entites.RealTime.VolumeByVenue;

public class VolumeByVenue : EntityBase
{
    public string Symbol { get; set; }
    public List<VolumeByVenueItem> Items { get; set; }
}
