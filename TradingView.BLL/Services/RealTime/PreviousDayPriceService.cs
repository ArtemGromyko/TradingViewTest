using Entites.Exceptions;
using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class PreviousDayPriceService : IPreviousDayPriceService
{
    private readonly IPreviousDayPriceRepository _previousDayPriceRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public PreviousDayPriceService(IPreviousDayPriceRepository previousDayPriceRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _previousDayPriceRepository = previousDayPriceRepository;
        _configuration = configuration;

        _httpClient = httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<PreviousDayPrice> GetPreviousDayPriceAsync(string symbol)
    {
        var previousDayPrice = await _previousDayPriceRepository.GetAsync((pdp) => pdp.Symbol!.ToUpper() == symbol.ToUpper());
        if (previousDayPrice is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:previousDayPriceUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            previousDayPrice = await response.Content.ReadAsAsync<PreviousDayPrice>();

            await _previousDayPriceRepository.AddAsync(previousDayPrice);
        }

        return previousDayPrice;
    }
}
