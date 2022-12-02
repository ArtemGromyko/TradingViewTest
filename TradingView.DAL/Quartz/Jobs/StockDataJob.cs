using Entites;
using Entites.StockFundamentals;
using Entites.StockProfile;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;
using System.Net;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.DAL.Quartz.Jobs;

public class StockDataJob : IJob
{
    private readonly IStockProfileApiService _stockProfileApiService;
    private readonly IStockFundamentalsApiService _stockFundamentalsApiService;
    private readonly ISymbolApiService _symbolApiService;

    private readonly IStockProfileRepository _stockProfileRepository;
    private readonly IStockFundamentalsRepository _stockFundamentalsRepository;
    private readonly ISymbolRepository _symbolRepository;

    private readonly IOptions<JobConfiguration> _jobConfiguration;

    public StockDataJob(IStockProfileApiService stockProfileApiService, IStockFundamentalsApiService stockFundamentalsApiService,
        ISymbolApiService symbolApiService, IStockProfileRepository stockProfileRepository, IStockFundamentalsRepository stockFundamentalsRepository,
        ISymbolRepository symbolRepository, IOptions<JobConfiguration> jobConfiguration)
    {
        _stockProfileApiService = stockProfileApiService;
        _stockFundamentalsApiService = stockFundamentalsApiService;
        _symbolApiService = symbolApiService;
        _stockProfileRepository = stockProfileRepository;
        _stockFundamentalsRepository = stockFundamentalsRepository;
        _symbolRepository = symbolRepository;
        _jobConfiguration = jobConfiguration;
    }

    public async Task<List<StockProfile>> ProcessStockProfileAsync(List<string> symbolNames)
      {
        var stockProfileTasks = new List<Task<(StockProfile, ResponseDto)>>();

        var skip = _jobConfiguration.Value.Skip;
        var take = _jobConfiguration.Value.Take;
        var delay = _jobConfiguration.Value.Delay;

        var fetchDelay = _jobConfiguration.Value.FetchDelay;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();

            foreach (var symbol in currentSymbols)
            {
                var stockProfileTask = _stockProfileApiService.FetchStockProfileAsync(symbol);
                await Task.Delay(fetchDelay);

                stockProfileTasks.Add(stockProfileTask);
            }

            skip += take;

            await Task.Delay(delay);
        }
        while (skip < symbolNames.Count);

        var stockProfiles = await Task.WhenAll(stockProfileTasks);

        var stockProfileSymbolsErrors = stockProfiles
            .Where((sp) => sp.Item2?.StatusCode == HttpStatusCode.TooManyRequests)
            .Select((s) => s.Item2?.Symbol)
            .ToList();

        var stockProfilesSucced = stockProfiles
            .Where((sp) => sp.Item2?.StatusCode == HttpStatusCode.OK)
            .Select((s) => s.Item1)
            .ToList();

        if (stockProfileSymbolsErrors.Count != 0)
        {
            await Task.Delay(delay);
            var newStockProfiles = await ProcessStockProfileAsync(stockProfileSymbolsErrors);

            stockProfilesSucced.AddRange(newStockProfiles);
        }

        return stockProfilesSucced;
    }

    public async Task<List<StockFundamentals>> ProcessStockfundamentalsAsync(List<string> symbolNames)
    {
        var stockFundamentalsTasks = new List<Task<(StockFundamentals, ResponseDto)>>();

        var skip = _jobConfiguration.Value.Skip;
        var take = _jobConfiguration.Value.Take;
        var delay = _jobConfiguration.Value.Delay;

        var fetchDelay = _jobConfiguration.Value.FetchDelay;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();

            foreach (var symbol in currentSymbols)
            {
                var stockFundamentalsTask = _stockFundamentalsApiService.FetchStockFundamentalsAsync(symbol);
                await Task.Delay(fetchDelay);

                stockFundamentalsTasks.Add(stockFundamentalsTask);
            }

            skip += take;

            await Task.Delay(delay);
        }
        while (skip < symbolNames.Count);

        var stockFundamentals = await Task.WhenAll(stockFundamentalsTasks);

        var stockFundamentalsSymbolsErrors = stockFundamentals
            .Where((sf) => sf.Item2?.StatusCode == HttpStatusCode.TooManyRequests)
            .Select((s) => s.Item2?.Symbol)
            .ToList();

        var stockFundamentalsSucceed = stockFundamentals
            .Where((sp) => sp.Item2?.StatusCode == HttpStatusCode.OK)
            .Select((s) => s.Item1)
            .ToList();

        if (stockFundamentalsSymbolsErrors.Count != 0)
        {
            await Task.Delay(delay);
            var stockFundamentalsRes = await ProcessStockfundamentalsAsync(stockFundamentalsSymbolsErrors);

            stockFundamentalsSucceed.AddRange(stockFundamentalsRes);
        }

        return stockFundamentalsSucceed;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _symbolRepository.DeleteAllAsync();

        var symbols = await _symbolApiService.FetchSymbolsAsync();

        await _symbolRepository.AddCollectionAsync(symbols);

        var symbolNames = symbols.Select((symbol) => symbol.Symbol).ToList();

        var stockProfiles = await ProcessStockProfileAsync(symbolNames);
        var stockFundamentals = await ProcessStockfundamentalsAsync(symbolNames);

        await Task.WhenAll(_stockProfileRepository.DeleteAllAsync(), _stockFundamentalsRepository.DeleteAllAsync());

        await _stockProfileRepository.AddCollectionAsync(stockProfiles);
        await _stockFundamentalsRepository.AddCollectionAsync(stockFundamentals);

        Log.Information("Done!");
    }
}
