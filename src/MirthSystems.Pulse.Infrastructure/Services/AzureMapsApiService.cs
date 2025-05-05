namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Azure;
    using Azure.Maps.Geolocation;
    using Azure.Maps.Search;
    using Azure.Maps.TimeZones;
    using MirthSystems.Pulse.Core.Interfaces;

    public class AzureMapsApiService : IAzureMapsApiService
    {
        public MapsGeolocationClient GeolocationClient { get; }
        public MapsSearchClient SearchClient { get; }
        public MapsTimeZoneClient TimeZonesClient { get; }

        public AzureMapsApiService(AzureKeyCredential azureMapsKeyCredential)
        {
            GeolocationClient = new MapsGeolocationClient(azureMapsKeyCredential);
            SearchClient = new MapsSearchClient(azureMapsKeyCredential);
            TimeZonesClient = new MapsTimeZoneClient(azureMapsKeyCredential);
        }
    }
}
