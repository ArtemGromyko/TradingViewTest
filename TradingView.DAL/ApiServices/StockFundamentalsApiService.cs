using Entites;
using Entites.StockFundamentals;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TradingView.DAL.Abstractions.ApiServices;

namespace TradingView.DAL.ApiServices;

public class StockFundamentalsApiService : IStockFundamentalsApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public StockFundamentalsApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<(StockFundamentals, ResponseDto)> FetchStockFundamentalsAsync(string symbol)
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockFundamentalsUrl"], symbol)}" +
                $"&token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var response = await _httpClient.GetAsync(url);
        var responseDto = new ResponseDto { StatusCode = response.StatusCode, Symbol = symbol };

        if (!response.IsSuccessStatusCode)
        {
            return (null, responseDto);
        }

        var stringResult = await response.Content.ReadAsStringAsync();

        var jsonParsed = JObject.Parse(stringResult);
        var item = jsonParsed[$"{symbol.ToUpper()}"];

        StockFundamentalsItem stockFundamentalsItem = null;

        if (item != null && item.Type != JTokenType.Null)
        {
            stockFundamentalsItem = jsonParsed[$"{symbol.ToUpper()}"].ToObject<StockFundamentalsItem>();
        }

        var stockFundamentals = new StockFundamentals { SymbolName = symbol, StockFundamentalsItem = stockFundamentalsItem };

        return (stockFundamentals, responseDto);
    }
} 
