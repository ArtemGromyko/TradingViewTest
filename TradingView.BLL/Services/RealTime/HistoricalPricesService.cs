using Entites.Exceptions;
using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class HistoricalPricesService : IHistoricalPricesService
{
    private readonly IHistoricalPricesRepository _historicalPricesRepository;

    private readonly IConfiguration _configuration;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HistoricalPricesService(IHistoricalPricesRepository historicalPricesRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory, HttpClient httpClient)
    {
        _historicalPricesRepository = historicalPricesRepository;
        _configuration = configuration;

        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<List<HistoricalPriceItem>> GetHistoricalPricesListAsync(string symbol)
    {
        var historicalPrice = await _historicalPricesRepository.GetAsync((hp) => hp.Symbol!.ToUpper() == symbol.ToUpper());
        if (historicalPrice is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:historicalPricesMaxUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var res = await response.Content.ReadAsAsync<List<HistoricalPriceItem>>();

            historicalPrice = new HistoricalPrice { Symbol = symbol, Items = res };
            await _historicalPricesRepository.AddAsync(historicalPrice);
        }

        var lastPrice = historicalPrice.Items[^1];
        var date = Convert.ToDateTime(lastPrice.Date);
        var currentDate = DateTime.Today;
        var previousDate = currentDate.AddDays(-1);

        if (!date.Equals(previousDate) && currentDate.DayOfWeek != DayOfWeek.Monday)
        {
            var range = "5d";

            var dateDifference = currentDate - date;

            if (dateDifference.Days <= 5)
            {
                range = "5d";
            }
            else if (dateDifference.Days >= 5 && dateDifference.Days <= 31)
            {
                range = "1m";
            }
            else if (dateDifference.Days >= 31 && dateDifference.Days <= 93)
            {
                range = "3m";
            }
            else if (dateDifference.Days >= 93 && dateDifference.Days <= 186)
            {
                range = "6m";
            }
            else
            {
                range = "1y";
            }

            await Task.Delay(3000);
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
               $"{string.Format(_configuration["IEXCloudUrls:historicalPricesUrl"], symbol, range)}" +
               $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var res = await response.Content.ReadAsAsync<List<HistoricalPriceItem>>();

            var newItems = historicalPrice.Items.UnionBy(res, x => x.Date).ToList();
            historicalPrice.Items = newItems;
            await _historicalPricesRepository.UpdateAsync(historicalPrice);
        }

        return historicalPrice.Items!;
    }
}
