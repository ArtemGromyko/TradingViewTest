using Entites.StockFundamentals.FinancialsAsReported;
using Microsoft.Extensions.Configuration;
using TradingView.DAL.Abstractions.ApiServices;

namespace TradingView.DAL.ApiServices;

public class FinancialsAsReportedApiService : IFinancialsAsReportedApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public FinancialsAsReportedApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<FinancialsAsReported> FetchFinancialsAsReportedAsync(string symbol)
    {
        var httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);

        var url = $"{_configuration["IEXCloudUrls:version"]}" +
               $"{string.Format(_configuration["IEXCloudUrls:financialsAsReportedUrl"], symbol)}" +
               $"?token={_configuration["Token"]}";

        var response = await httpClient.GetAsync(url);

        var financialsAsReportedItems = await response.Content.ReadAsAsync<List<FinancialsAsReportedItem>>();

        return new FinancialsAsReported { Symbol = symbol, FinancialsAsReportedItems = financialsAsReportedItems };
    }
}
