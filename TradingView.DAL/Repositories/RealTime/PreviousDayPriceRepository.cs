using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class PreviousDayPriceRepository : RepositoryBase<PreviousDayPrice>, IPreviousDayPriceRepository
{
    public PreviousDayPriceRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:PreviousDayPriceCollectionName"])
    {
    }
}
