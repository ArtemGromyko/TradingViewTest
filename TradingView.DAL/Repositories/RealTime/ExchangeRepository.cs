using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories;

public class ExchangeRepository : RepositoryBase<ExchangeInfo>, IExchangeRepository
{
    public ExchangeRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:ExchangeCollectionName"])
    {
    }
}
