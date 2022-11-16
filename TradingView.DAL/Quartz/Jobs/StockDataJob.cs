using Entites.StockFundamentals;
using Entites.StockProfile;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.DAL.Quartz.Jobs;

public class StockDataJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StockDataJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var symbolRepository = scope.ServiceProvider.GetService<ISymbolRepository>();
        var stockProfileRepository = scope.ServiceProvider.GetService<IStockProfileRepository>();
        var stockFundamentalsRepository = scope.ServiceProvider.GetService<IStockFundamentalsRepository>();

        var symbolApiService = scope.ServiceProvider.GetService<ISymbolApiService>();
        var stockProfileApiService = scope.ServiceProvider.GetService<IStockProfileApiService>();
        var stockFundamentalsApiService = scope.ServiceProvider.GetService<IStockFundamentalsApiService>();

        await symbolRepository.DeleteAllAsync();
        await stockProfileRepository.DeleteAllAsync();
        await stockFundamentalsRepository.DeleteAllAsync();

        var symbols = await symbolApiService.FetchSymbolsAsync();

        await symbolRepository.AddCollectionAsync(symbols.Take(100));

        var symbolNames = symbols.Select(symbol => symbol.Symbol).Take(100).ToList();

        var stockProfileTasks = new List<Task<StockProfile>>();
        var stockFundamentalsTasks = new List<Task<StockFundamentals>>();


        var skip = 0;
        var take = 5;
        var delay = 1500;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();

            foreach (var symbol in currentSymbols)
            {
                var stockProfileTask = stockProfileApiService.FetchStockProfileAsync(symbol);
                await Task.Delay(250);
                var stockFundamentalsTask = stockFundamentalsApiService.FetchStockFundamentalsAsync(symbol);
                await Task.Delay(250);

                stockFundamentalsTasks.Add(stockFundamentalsTask);
                stockProfileTasks.Add(stockProfileTask);
            }

            skip += 5;

            await Task.Delay(delay);

            //delay += 100;
        }
        while (skip < 100);

        var stockProfiles = await Task.WhenAll(stockProfileTasks);
        var stockFundamentals = await Task.WhenAll(stockFundamentalsTasks);

        await stockProfileRepository.AddCollectionAsync(stockProfiles);
        await stockFundamentalsRepository.AddCollectionAsync(stockFundamentals);
    }
}
