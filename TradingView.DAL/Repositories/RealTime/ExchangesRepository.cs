using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class ExchangesRepository : RepositoryBase<ExchangeInfo>, IExchangesRepository
{
    public ExchangesRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:ExchangesCollectionName"])
    {
    }
}
