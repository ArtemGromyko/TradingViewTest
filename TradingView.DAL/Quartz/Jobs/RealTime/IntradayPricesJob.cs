using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.DAL.Quartz.Jobs.RealTime;

public class IntradayPricesJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public IntradayPricesJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetService<IIntradayPricesRepository>();
            await repository.DeleteAllAsync();
        }
    }
}
