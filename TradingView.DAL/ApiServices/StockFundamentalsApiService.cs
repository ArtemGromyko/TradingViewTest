using Entites.StockFundamentals;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TradingView.DAL.Abstractions.ApiServices;

namespace TradingView.DAL.ApiServices;

public class StockFundamentalsApiService : IStockFundamentalsApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public StockFundamentalsApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<StockFundamentals> FetchStockFundamentalsAsync(string symbol)
    {
        var httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);

        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockFundamentalsUrl"], symbol)}" +
                $"&token={_configuration["Token"]}";

        var response = await httpClient.GetAsync(url);
        var stringResult = await response.Content.ReadAsStringAsync();

        var jsonParsed = JObject.Parse(stringResult);
        var item = jsonParsed[$"{symbol.ToUpper()}"];

        StockFundamentalsItem stockFundamentalsItem = null;

        if (item != null && item.Type != JTokenType.Null)
        {
            stockFundamentalsItem = jsonParsed[$"{symbol.ToUpper()}"].ToObject<StockFundamentalsItem>();
        }

        var stockFundamentals = new StockFundamentals { SymbolName = symbol, StockFundamentalsItem = stockFundamentalsItem };

        return stockFundamentals;
    }
} 
