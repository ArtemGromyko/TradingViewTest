using Entites.Exceptions;
using Entites.RealTime.VolumeByVenue;
using Microsoft.Extensions.Configuration;
using TradingView.BLL.Abstractions.RealTime;
using TradingView.DAL.Abstractions.Repositories.RealTime;

namespace TradingView.BLL.Services.RealTime;

public class VolumeByVenueService : IVolumeByVenueService
{
    private readonly IVolumeByVenueRepository _volumeByVenueRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public VolumeByVenueService(IVolumeByVenueRepository volumeByVenueRepository, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _volumeByVenueRepository = volumeByVenueRepository;
        _configuration = configuration;

        _httpClient = httpClientFactory.CreateClient(_configuration["HttpClientName"]);
    }
    public async Task<List<VolumeByVenueItem>> GetVolumesByVenueAsync(string symbol)
    {
        var volumeByVenue = await _volumeByVenueRepository.GetAsync(v => v.Symbol!.ToUpper() == symbol.ToUpper());
        if (volumeByVenue is null)
        {
            var url = $"{_configuration["IEXCloudUrls:version"]}" +
                $"{string.Format(_configuration["IEXCloudUrls:volumeByVenueUrl"], symbol)}" +
                $"?token={Environment.GetEnvironmentVariable("PUBLISHABLE_TOKEN")}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new IexCloudException(response);
            }

            var volumesByVenue = await response.Content.ReadAsAsync<List<VolumeByVenueItem>>();

            var newVolumeByVenue = new VolumeByVenue { Symbol = symbol, Items = volumesByVenue };

            await _volumeByVenueRepository.AddAsync(newVolumeByVenue);

            return volumesByVenue;
        }

        return volumeByVenue.Items!;
    }
}
