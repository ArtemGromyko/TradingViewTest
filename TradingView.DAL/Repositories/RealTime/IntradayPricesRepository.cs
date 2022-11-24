using Entites.RealTime.IntradayPrice;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class IntradayPricesRepository : RepositoryBase<IntradayPrice>, IIntradayPricesRepository
{
    public IntradayPricesRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:IntradayPricesCollectionName"])
    {
    }
}
