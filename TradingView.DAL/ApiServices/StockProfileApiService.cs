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

    private readonly HttpClient _httpClient;

    public StockProfileApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<(StockProfile, ResponseDto)> FetchStockProfileAsync(SymbolInfo symbol)
    { 
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockProfileUrl"], symbol.Symbol)}" +
                $"&token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var response = await _httpClient.GetAsync(url);
        var responseDto = new ResponseDto { StatusCode = response.StatusCode, Symbol = symbol.Symbol };

        if (!response.IsSuccessStatusCode)
        {
            return (null, responseDto);
        }

        var stringResult = await response.Content.ReadAsStringAsync();

        var jsonParsed = JObject.Parse(stringResult);
        var item = jsonParsed[$"{symbol.Symbol.ToUpper()}"];

        StockProfileItem stockProfileItem = null;

        if (item != null && item.Type != JTokenType.Null)
        {
            stockProfileItem = jsonParsed[$"{symbol.Symbol.ToUpper()}"].ToObject<StockProfileItem>();
        }

        var stockProfile = new StockProfile { SymbolName = symbol.Symbol.ToUpper(), StockProfileItem = stockProfileItem };

        symbol.Logo = stockProfile?.StockProfileItem?.Logo?.Url;

        return (stockProfile, responseDto);
    }
}
