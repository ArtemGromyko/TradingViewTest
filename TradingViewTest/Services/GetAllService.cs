using Newtonsoft.Json.Linq;
using TradingViewTest.Models;
using TradingViewTest.Models.StockFundamentals;
using TradingViewTest.Models.StockProfile;

namespace TradingViewTest.Services;

public class GetAllService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GetAllService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient(configuration["HttpClientName"]);
    }

    public async Task<List<SymbolInfo>> GetSymbolsAsync()
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{_configuration["IEXCloudUrls:symbolUrl"]}" +
                $"?token={_configuration["Token"]}";

        var response = await _httpClient.GetAsync(url);
        var symbols = await response.Content.ReadAsAsync<List<SymbolInfo>>();

        return symbols;
    }

    public async Task<StockProfile> GetStockProfileAsync(string symbol)
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockProfileUrl"], symbol)}" +
                $"&token={_configuration["Token"]}";

        var response = await _httpClient.GetAsync(url);
        var stringResult = await response.Content.ReadAsStringAsync();
        var jsonParsed = JObject.Parse(stringResult);
        var stockProfileItem = jsonParsed[$"{symbol.ToUpper()}"].ToObject<StockProfileItem>();

        var stockProfile = new StockProfile { SymbolName = symbol, StockProfileItem = stockProfileItem };

        return stockProfile;
    }

    public async Task<StockFundamentals> GetStockFundamentalsAsync(string symbol)
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockFundamentalsUrl"], symbol)}" +
                $"&token={_configuration["Token"]}";

        var response = await _httpClient.GetAsync(url);
        var stringResult = await response.Content.ReadAsStringAsync();
        var jsonParsed = JObject.Parse(stringResult);
        var stockFundamentalsItem = jsonParsed[$"{symbol.ToUpper()}"].ToObject<StockFundamentalsItem>();

        var stockFundamentals = new StockFundamentals { SymbolName = symbol, StockFundamentalsItem = stockFundamentalsItem };

        return stockFundamentals;
    }

    public async Task GetAllAsync()
    {
        var symbols = await GetSymbolsAsync();
        var symbolNames = symbols.Select(symbol => symbol.Symbol).ToList();

        var skip = 0;
        var take = 9;
        var delay = 1000;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();


            await Task.Delay(delay);
            delay += 1000;
        }
        while (skip < symbols.Count);
    }
}
