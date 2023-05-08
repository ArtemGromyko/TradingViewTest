using Entites;
using Entites.Exceptions;
using Entites.RealTime;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TradingView.BLL.Abstractions.RealTime;

namespace TradingView.BLL.Services.RealTime
{
    public class RealTimeService : IRealTimeService
    {
        private readonly IConfiguration _configuration;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public RealTimeService(IConfiguration configuration, IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _configuration = configuration;

            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient(configuration["httpClientName"]);
        }

        public async Task<RealTimeDto> GetRealTimeDataAsync(string symbol)
        {
            var httpClient = _httpClientFactory.CreateClient(_configuration["HttpClientName"]);

            var urlRealTime = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:realTimeUrl"], symbol)}" +
                $"&token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await httpClient.GetAsync(urlRealTime);

            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var stringResult = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(stringResult);

            RealTimeDto result = new()
            {
                Symbol = symbol.ToUpper(),
                Item = json[$"{symbol.ToUpper()}"].ToObject<RealTimeItem>()
            };

            return result;
        }
    }
}
