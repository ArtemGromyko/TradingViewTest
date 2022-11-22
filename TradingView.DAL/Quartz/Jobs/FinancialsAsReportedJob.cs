using Entites.StockFundamentals.FinancialsAsReported;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.DAL.Quartz.Jobs;

public class FinancialsAsReportedJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FinancialsAsReportedJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var symbolRepository = scope.ServiceProvider.GetService<ISymbolRepository>();
        var financialsAsReportedRepository = scope.ServiceProvider.GetService<IFinancialsAsReportedRepository>();

        var symbolApiService = scope.ServiceProvider.GetService<ISymbolApiService>();
        var financialsAsReportedApiService = scope.ServiceProvider.GetService<IFinancialsAsReportedApiService>();

        var symbols = await symbolRepository.GetAllAsync();

        if (symbols.Count == 0)
        {
            symbols = await symbolApiService.FetchSymbolsAsync();
            await symbolRepository.AddCollectionAsync(symbols);
        }

        var symbolNames = symbols.Select(symbol => symbol.Name).Take(100).ToList();

        var financialsAsReportedTasks = new List<Task<FinancialsAsReported>>();

        var skip = 0;
        var take = 5;
        var delay = 1500;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();

            foreach (var symbol in currentSymbols)
            {
                var financialsAsReported = financialsAsReportedApiService.FetchFinancialsAsReportedAsync(symbol);
                await Task.Delay(250);
            }

            skip += 5;

            await Task.Delay(delay);
        }
        while (skip < 100);

        var financialsAsReportedList = await Task.WhenAll(financialsAsReportedTasks);

        await financialsAsReportedRepository.AddCollectionAsync(financialsAsReportedList);
    }
}
