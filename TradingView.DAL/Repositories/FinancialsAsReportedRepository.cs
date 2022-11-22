using Entites.StockFundamentals.FinancialsAsReported;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TradingView.DAL.Abstractions.Repositories;
using TradingView.DAL.Settings;

namespace TradingView.DAL.Repositories;

public class FinancialsAsReportedRepository : RepositoryBase<FinancialsAsReported>, IFinancialsAsReportedRepository
{
    public FinancialsAsReportedRepository(IOptions<DatabaseSettings> settings, IConfiguration configuration)
        : base(settings, configuration["MongoDBCollectionNames:FinancialsAsReportedCollectionName"])
    {
    }
}
