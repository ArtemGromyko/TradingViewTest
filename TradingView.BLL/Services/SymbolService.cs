using Entites;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions;
using TradingView.DAL.Abstractions.ApiServices;
using TradingView.DAL.Abstractions.Repositories;

namespace TradingView.BLL.Services;

public class SymbolService : ISymbolService
{
    private readonly ISymbolRepository _symbolRepository;
    private readonly ISymbolApiService _symbolApiService;

    public SymbolService(ISymbolRepository symbolRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory, ISymbolApiService symbolApiService)
    {
        _symbolRepository = symbolRepository;
        _symbolApiService = symbolApiService;
    }

    public async Task<List<SymbolInfo>> GetSymbolsAsync()
    {
        var symbols = await _symbolRepository.GetAllAsync();
        if (symbols.Count == 0)
        {
            symbols = await _symbolApiService.FetchSymbolsAsync();

            await _symbolRepository.AddCollectionAsync(symbols.Take(100));
        }

        return symbols;
    }

}
