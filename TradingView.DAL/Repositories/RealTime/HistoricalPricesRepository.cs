using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TradingView.DAL.Abstractions.Repositories.RealTime;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories.RealTime;

public class HistoricalPricesRepository : RepositoryBase<HistoricalPrice>, IHistoricalPricesRepository
{
    public HistoricalPricesRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:HistoricalPricesCollectionName"])
    {
    }

    public async Task UpdateAsync(HistoricalPrice historicalPrice)
    {
        var filter = Builders<HistoricalPrice>.Filter.Eq(s => s.Symbol, historicalPrice.Symbol);
        var result = await _collection.ReplaceOneAsync(filter, historicalPrice);
    }
}
