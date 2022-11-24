using Entites.Exceptions;
using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Contracts;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class ExchangeService : IExchangeService
{
    private readonly IExchangeRepository _exchangeRepository;

    private readonly IConfiguration _configuration;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public ExchangeService(IExchangeRepository exchangeRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _exchangeRepository = exchangeRepository;
        _configuration = configuration;

        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<List<ExchangeInfo>> GetExchangesAsync()
    {
        var exchanges = await _exchangeRepository.GetAllAsync();
        if (exchanges.Count == 0)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{_configuration["IEXCloudUrls:exchangeUrl"]}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            exchanges = await response.Content.ReadAsAsync<List<ExchangeInfo>>();

            await _exchangeRepository.AddCollectionAsync(exchanges);
        }

        return exchanges;
    }
}
