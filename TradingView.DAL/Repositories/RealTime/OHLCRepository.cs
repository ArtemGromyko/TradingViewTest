using Entites.RealTime.OHLC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class OHLCRepository : RepositoryBase<OHLC>, IOHLCRepository
{
    public OHLCRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:OHLCCollectionName"])
    {
    }
}
