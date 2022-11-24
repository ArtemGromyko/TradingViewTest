using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.DAL.Quartz.Jobs.RealTime;

public class VolumeByVenueJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public VolumeByVenueJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetService<IVolumeByVenueRepository>();
            await repository.DeleteAllAsync();
        }
    }
}
