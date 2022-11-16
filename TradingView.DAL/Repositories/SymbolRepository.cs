using Entites;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories;

public class SymbolRepository : RepositoryBase<SymbolInfo>, ISymbolRepository
{
    public SymbolRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:SymbolCollectionName"])
    {
    }
}
