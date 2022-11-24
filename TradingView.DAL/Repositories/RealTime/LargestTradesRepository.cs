using Entites.RealTime.LargestTrade;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class LargestTradesRepository : RepositoryBase<LargestTrade>, ILargestTradesRepository
{
    public LargestTradesRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:LargestTradesCollectionName"])
    {
    }
}
