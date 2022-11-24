using Entites.Exceptions;
using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class DelayedQuoteService : IDelayedQuoteService
{
    private readonly IDelayedQuoteRepository _delayedQuoteRepository;

    private readonly IConfiguration _configuration;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public DelayedQuoteService(IDelayedQuoteRepository delayedQuoteRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _delayedQuoteRepository = delayedQuoteRepository;
        _configuration = configuration;

        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<DelayedQuote> GetDelayedQuoteAsync(string symbol)
    {
        var delayedQuote = await _delayedQuoteRepository.GetAsync((d) => d.Symbol!.ToUpper() == symbol.ToUpper());
        if (delayedQuote is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:delayedQuoteUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            delayedQuote = await response.Content.ReadAsAsync<DelayedQuote>();

            await _delayedQuoteRepository.AddAsync(delayedQuote);
        }

        return delayedQuote;
    }
}
