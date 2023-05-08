using Entites.Exceptions;
using Entites.RealTime;
using Entites.RealTime.VolumeByVenue;
using Entites.StockProfile;
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
        var urlForMaxValue = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:historicalPricesMaxUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var urlForCurrentDate = $"{_configuration["IEXCloudUrls:version"]}" +
            $"/stock/{symbol}/chart/date/{DateTime.UtcNow.Year}" +
            $"{(DateTime.UtcNow.Month < 10 ? "0" + DateTime.UtcNow.Month : DateTime.UtcNow.Month)}" +
            $"{(DateTime.UtcNow.Day < 10 ? "0" + DateTime.UtcNow.Day : DateTime.UtcNow.Day)}" +
            $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var urlForVolume = $"{_configuration["IEXCloudUrls:version"]}" +
            $"/stock/{symbol}/volume-by-venue" +
            $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";


        var responseWithCurrentDate = await _httpClient.GetAsync(urlForCurrentDate);
        var responseVolume = await _httpClient.GetAsync(urlForVolume);

        if (!responseWithCurrentDate.IsSuccessStatusCode)
        {
            throw new IexCloudException(responseWithCurrentDate);
        }
        else if (!responseVolume.IsSuccessStatusCode)
        {
            throw new IexCloudException(responseVolume);
        }

        var resWithCurrentDate = await responseWithCurrentDate.Content.ReadAsAsync<List<HistoricalPriceItem>>();
        var volume = await responseVolume.Content.ReadAsAsync<List<VolumeByVenueItem>>();


        var historicalPrice = await _historicalPricesRepository.GetAsync((hp) => hp.Symbol!.ToUpper() == symbol.ToUpper());
        if (historicalPrice is null)
        {
            var response = await _httpClient.GetAsync(urlForMaxValue);

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

        if (!date.Equals(previousDate))
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

        if (resWithCurrentDate.Count != 0)
        {
            historicalPrice.Items.Add(new HistoricalPriceItem
            {
                Date = resWithCurrentDate[0].Date,
                Low = resWithCurrentDate.Where(_ => _.Low != 0).Min(_ => _.Low),
                High = resWithCurrentDate.Max(_ => _.High),
                Open = resWithCurrentDate[1].Open,
                // we need to think how to determine closing time more correct
                Close = DateTime.UtcNow > new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 20, 0, 0)
                    ? resWithCurrentDate[^2].Close
                    : resWithCurrentDate[^1].Close,
                Volume = volume.Sum(_ => _.Volume)
            });
        }

        return historicalPrice.Items!;
    }
}
