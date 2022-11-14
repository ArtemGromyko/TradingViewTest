using Entites.StockProfile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories;

public class StockProfileRepository : RepositoryBase<StockProfile>, IStockProfileRepository
{
    public StockProfileRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:StockProfileCollectionName"])
    {
    }
}
