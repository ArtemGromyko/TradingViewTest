using Entites;
using Entites.StockFundamentals;
using Entites.StockProfile;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace TradingView.BLL.Services;

public class GetAllService
{
    private readonly int RequestDelay = 200;

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
        await Task.Delay(RequestDelay);

        return symbols;
    }

    public async Task<StockProfile> GetStockProfileAsync(string symbol)
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockProfileUrl"], symbol)}" +
                $"&token={_configuration["Token"]}";

        var response = await _httpClient.GetAsync(url);
        var stringResult = await response.Content.ReadAsStringAsync();

        //await Task.Delay(RequestDelay);

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

    public async Task<StockFundamentals> GetStockFundamentalsAsync(string symbol)
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:stockFundamentalsUrl"], symbol)}" +
                $"&token={_configuration["Token"]}";

        var response = await _httpClient.GetAsync(url);
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

    public async Task<List<AllDto>> GetAllAsync()
    {
        /*var fixture = new Fixture();
        var response = fixture.CreateMany<AllDto>(11000).ToList();
        return response;*/

        var symbols = await GetSymbolsAsync();
        var symbolNames = symbols.Select(symbol => symbol.Symbol).Take(1000).ToList();

        var stockProfileTasks = new List<Task<StockProfile>>();
        var stockFundamentalsTasks = new List<Task<StockFundamentals>>();


        var skip = 0;
        var take = 5;
        var delay = 500;

        do
        {
            var currentSymbols = symbolNames.Skip(skip).Take(take).ToList();

            foreach (var symbol in currentSymbols)
            {
                var stockProfileTask = GetStockProfileAsync(symbol);
                await Task.Delay(180);
                var stockFundamentalsTask = GetStockFundamentalsAsync(symbol);
                await Task.Delay(180);

                stockFundamentalsTasks.Add(stockFundamentalsTask);
                stockProfileTasks.Add(stockProfileTask);
            }

            skip += 5;

            if (stockFundamentalsTasks.Count % 600 == 0)
            {
                delay += 200;
            }

            await Task.Delay(delay);

            //delay += 100;
        }
        while (skip < 1000);

        var stockProfiles = await Task.WhenAll(stockProfileTasks);
        var stockFundamentals = await Task.WhenAll(stockFundamentalsTasks);

        var orderedStockProfiles = stockProfiles.OrderBy(sp => sp.SymbolName).ToList();
        var orderStockFundamentals = stockFundamentals.OrderBy(sf => sf.SymbolName).ToList();

        var allDtos = orderedStockProfiles.Zip(orderStockFundamentals, (sp, sf) =>
            new AllDto { SymbolName = sp.SymbolName, StockProfile = sp.StockProfileItem, StockFundamentals = sf.StockFundamentalsItem });

        return allDtos.ToList();
    }
}
