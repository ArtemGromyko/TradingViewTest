using Entites.StockProfile;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TradingView.DAL.Abstractions.ApiServices;

namespace TradingView.DAL.ApiServices;

public class StockProfileApiService : IStockProfileApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public StockProfileApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<StockProfile> FetchStockProfileAsync(string symbol)
    {
        var httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);

        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockProfileUrl"], symbol)}" +
                $"&token={_configuration["Token"]}";

        var response = await httpClient.GetAsync(url);
        var stringResult = await response.Content.ReadAsStringAsync();

        var jsonParsed = JObject.Parse(stringResult);
        var item = jsonParsed[$"{symbol.ToUpper()}"];

        StockProfileItem stockProfileItem = null;

        if (item != null && item.Type != JTokenType.Null)
        {
            stockProfileItem = jsonParsed[$"{symbol.ToUpper()}"].ToObject<StockProfileItem>();
        }

        var stockProfile = new StockProfile { SymbolName = symbol, StockProfileItem = stockProfileItem };

        return stockProfile;
    }
}
