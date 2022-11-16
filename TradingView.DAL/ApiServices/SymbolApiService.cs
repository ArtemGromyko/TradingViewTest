using Entites;
using Microsoft.Extensions.Configuration;
using TradingView.DAL.Abstractions.ApiServices;

namespace TradingView.DAL.ApiServices;

public class SymbolApiService : ISymbolApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public SymbolApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<SymbolInfo>> FetchSymbolsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);

        var url = $"{_configuration["IEXCloudUrls:version"]}" +
               $"{_configuration["IEXCloudUrls:symbolUrl"]}" +
               $"?token={_configuration["Token"]}";

        var response = await httpClient.GetAsync(url);

        return await response.Content.ReadAsAsync<List<SymbolInfo>>();
    }
}
