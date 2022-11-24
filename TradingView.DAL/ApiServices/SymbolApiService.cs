using Entites;
using Entites.Exceptions;
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
               $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new IexCloudException(response);
        }

        return await response.Content.ReadAsAsync<List<SymbolInfo>>();
    }
}
