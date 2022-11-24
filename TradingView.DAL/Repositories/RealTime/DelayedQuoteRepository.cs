using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class DelayedQuoteRepository : RepositoryBase<DelayedQuote>, IDelayedQuoteRepository
{
    public DelayedQuoteRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:DelayedQuoteCollectionName"])
    {
    }
}
