using Entites.Exceptions;
using Entites.RealTime.IntradayPrice;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class IntradayPricesService : IIntradayPricesService
{
    private readonly IIntradayPricesRepository _intradayPricesRepository;

    private readonly IConfiguration _configuration;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public IntradayPricesService(IIntradayPricesRepository intradayPricesRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _intradayPricesRepository = intradayPricesRepository;
        _configuration = configuration;

        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<List<IntradayPriceItem>> GetIntradayPricesListAsync(string symbol)
    {
        var intradayPrice = await _intradayPricesRepository.GetAsync((ip) => ip.Symbol!.ToUpper() == symbol.ToUpper());
        if (intradayPrice is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:intradayPricesUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var intradayPriceItems = await response.Content.ReadAsAsync<List<IntradayPriceItem>>();

            var newIntradayPrice = new IntradayPrice { Symbol = symbol, Items = intradayPriceItems };
            await _intradayPricesRepository.AddAsync(newIntradayPrice);

            return intradayPriceItems;
        }

        return intradayPrice.Items!;
    }
}
