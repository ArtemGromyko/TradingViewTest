using Entites;
using Entites.StockFundamentals.FinancialsAsReported;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;
using System.Net;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.DAL.Quartz.Jobs;

public class FinancialsAsReportedJob : IJob
{
    private readonly ISymbolApiService _symbolApiService;
    private readonly IFinancialsAsReportedApiService _financialsAsReportedApiService;

    private readonly ISymbolRepository _symbolRepository;
    private readonly IFinancialsAsReportedRepository _financialsAsReportedRepository;

    private readonly IOptions<JobConfiguration> _jobConfiguration;

    public FinancialsAsReportedJob(ISymbolApiService symbolApiService, IFinancialsAsReportedApiService financialsAsReportedApiService,
        ISymbolRepository symbolRepository, IFinancialsAsReportedRepository financialsAsReportedRepository, IOptions<JobConfiguration> jobConfiguration)
    {
        _symbolApiService = symbolApiService;
        _financialsAsReportedApiService = financialsAsReportedApiService;
        _symbolRepository = symbolRepository;
        _financialsAsReportedRepository = financialsAsReportedRepository;
        _jobConfiguration = jobConfiguration;
    }

    public async Task<List<FinancialsAsReported>> ProcessFinancialsAsReportedAsync(List<string> symbolNames)
    {
        var financialsAsReportedTasks = new List<Task<(FinancialsAsReported, ResponseDto)>>();

        var skip = _jobConfiguration.Value.Skip;
        var take = _jobConfiguration.Value.Take;
        var delay = _jobConfiguration.Value.Delay;

        var fetchDelay = _jobConfiguration.Value.FetchDelay;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();

            foreach (var symbol in currentSymbols)
            {
                var stockFundamentalsTask = _financialsAsReportedApiService.FetchFinancialsAsReportedAsync(symbol);
                await Task.Delay(fetchDelay);

                financialsAsReportedTasks.Add(stockFundamentalsTask);
            }

            skip += take;

            await Task.Delay(delay);
        }
        while (skip < symbolNames.Count);

        var financialsAsReported = await Task.WhenAll(financialsAsReportedTasks);

        var financialsAsReportedErrors = financialsAsReported
            .Where((sf) => sf.Item2?.StatusCode == HttpStatusCode.TooManyRequests)
            .Select((s) => s.Item2?.Symbol)
            .ToList();

        var financialsAsReportedSucceed = financialsAsReported
            .Where((sp) => sp.Item2?.StatusCode == HttpStatusCode.OK)
            .Select((s) => s.Item1)
            .ToList();

        if (financialsAsReportedErrors.Count != 0)
        {
            await Task.Delay(delay);
            var newFinancialsAsReported = await ProcessFinancialsAsReportedAsync(financialsAsReportedErrors);

            financialsAsReportedSucceed.AddRange(newFinancialsAsReported);
        }

        return financialsAsReportedSucceed;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var symbols = await _symbolRepository.GetAllAsync();

        if (symbols.Count == 0)
        {
            symbols = await _symbolApiService.FetchSymbolsAsync();
            await _symbolRepository.AddCollectionAsync(symbols);
        }

        var symbolNames = symbols.Select((symbol) => symbol.Symbol).ToList();

        var financialsAsReportedList = await ProcessFinancialsAsReportedAsync(symbolNames);

        await _financialsAsReportedRepository.DeleteAllAsync();

        await _financialsAsReportedRepository.AddCollectionAsync(financialsAsReportedList);

        Log.Information("Done!");
    }
}
