namespace MirthSystems.Pulse.Core.Interfaces
{
    using Azure.Maps.Geolocation;
    using Azure.Maps.Search;
    using Azure.Maps.TimeZones;

    public interface IAzureMapsApiService
    {
        MapsGeolocationClient GeolocationClient { get; }
        MapsSearchClient SearchClient { get; }
        MapsTimeZoneClient TimeZonesClient { get; }
    }
}
