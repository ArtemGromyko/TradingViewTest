using Entites;
using Entites.Exceptions;
using Entites.StockFundamentals.FinancialsAsReported;
using Microsoft.Extensions.Configuration;
using System.Net;
using TradingView.DAL.Abstractions.ApiServices;

namespace TradingView.DAL.ApiServices;

public class FinancialsAsReportedApiService : IFinancialsAsReportedApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public FinancialsAsReportedApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

        _httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }

    public async Task<(FinancialsAsReported, ResponseDto)> FetchFinancialsAsReportedAsync(string symbol)
    {
        var url = $"{_configuration["IEXCloudUrls:version"]}" +
               $"{string.Format(_configuration["IEXCloudUrls:financialsAsReportedUrl"], symbol)}" +
               $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

        var response = await _httpClient.GetAsync(url);
        var respnseDto = new ResponseDto { StatusCode = HttpStatusCode.OK, Symbol = symbol };
        var financialsAsreported = new FinancialsAsReported { Symbol = symbol.ToUpper() };

        if (!response.IsSuccessStatusCode)
        {
            return (financialsAsreported, respnseDto);
        }

        var financialsAsReportedItems = await response.Content.ReadAsAsync<List<FinancialsAsReportedItem>>();
        financialsAsreported.FinancialsAsReportedItems = financialsAsReportedItems;

        return (financialsAsreported, respnseDto);
    }
}
