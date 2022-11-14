using Entites.StockFundamentals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories;

public class StockFundamentalsRepository : RepositoryBase<StockFundamentals>, IStockFundamentalsRepository
{
    public StockFundamentalsRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:StockFundamentalsCollectionName"])
    {
    }
}
