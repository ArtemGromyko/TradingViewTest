using Entites.RealTime.VolumeByVenue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class VolumeByVenueRepository : RepositoryBase<VolumeByVenue>, IVolumeByVenueRepository
{
    public VolumeByVenueRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:VolumeByVenueCollectionName"])
    {
    }
}
