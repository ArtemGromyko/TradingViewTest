using Entites.Exceptions;
using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class PriceOnlyService : IPriceOnlyService
{
    private readonly IPriceOnlyRepository _priceOnlyRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public PriceOnlyService(IConfiguration configuration,
        IHttpClientFactory httpClientFactory, IPriceOnlyRepository priceOnlyRepository)
    {
        _priceOnlyRepository = priceOnlyRepository;
        _configuration = configuration;

        _httpClient = httpClientFactory.CreateClient(_configuration["HttpClientName"]);
        _priceOnlyRepository = priceOnlyRepository;
    }

    public async Task<double> GetPriceOnlyAsync(string symbol)
    {
        var priceOnly = await _priceOnlyRepository.GetAsync((po) => po.Symbol!.ToUpper() == symbol.ToUpper());
        if (priceOnly is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:priceOnlyUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var price = await response.Content.ReadAsAsync<double>();

            var newPriceOnly = new PriceOnly { Symbol = symbol, Price = price };
            await _priceOnlyRepository.AddAsync(newPriceOnly);

            priceOnly = newPriceOnly;
        }


        return priceOnly.Price;
    }
}
