using Entites;
using Entites.StockProfile;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
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

    public async Task<(StockProfile, ResponseDto)> FetchStockProfileAsync(string symbol)
    {
        var httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
        var str = Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN");
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockProfileUrl"], symbol)}" +
                $"&token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var response = await httpClient.GetAsync(url);
        var responseDto = new ResponseDto { StatusCode = response.StatusCode, Symbol = symbol };

        if (!response.IsSuccessStatusCode)
        {
            return (null, responseDto);
        }

        var stringResult = await response.Content.ReadAsStringAsync();

        var jsonParsed = JObject.Parse(stringResult);
        var item = jsonParsed[$"{symbol.ToUpper()}"];

        StockProfileItem stockProfileItem = null;

        if (item != null && item.Type != JTokenType.Null)
        {
            stockProfileItem = jsonParsed[$"{symbol.ToUpper()}"].ToObject<StockProfileItem>();
        }

        var stockProfile = new StockProfile { SymbolName = symbol, StockProfileItem = stockProfileItem };

        return (stockProfile, responseDto);
    }
}
