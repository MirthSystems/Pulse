namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Azure;
    using Azure.Maps.Geolocation;
    using Azure.Maps.Search;
    using Azure.Maps.TimeZones;
    using MirthSystems.Pulse.Core.Interfaces;

    /// <summary>
    /// Service that provides access to Azure Maps APIs for geospatial functionality.
    /// </summary>
    /// <remarks>
    /// <para>This service encapsulates the Azure Maps SDK clients for various mapping and location services:</para>
    /// <para>- Geolocation: IP-based location detection</para>
    /// <para>- Search: Geocoding, reverse geocoding, and place search</para>
    /// <para>- Time Zones: Time zone information for global locations</para>
    /// <para>The service is registered as a singleton to reuse the client connections.</para>
    /// </remarks>
    public class AzureMapsApiService : IAzureMapsApiService
    {
        /// <summary>
        /// Gets the Azure Maps Geolocation client for IP-based and device location services.
        /// </summary>
        /// <remarks>
        /// <para>Used to determine a user's location from their IP address or device signals.</para>
        /// <para>Example: Automatic region detection when user hasn't provided explicit location.</para>
        /// </remarks>
        public MapsGeolocationClient GeolocationClient { get; }

        /// <summary>
        /// Gets the Azure Maps Search client for geocoding and place search operations.
        /// </summary>
        /// <remarks>
        /// <para>Provides address search, geocoding, and reverse geocoding capabilities.</para>
        /// <para>Primary uses include:</para>
        /// <para>- Converting addresses to geographic coordinates (geocoding)</para>
        /// <para>- Finding nearby venues within a specified radius</para>
        /// <para>- Standardizing address formats for consistency</para>
        /// </remarks>
        public MapsSearchClient SearchClient { get; }

        /// <summary>
        /// Gets the Azure Maps Time Zones client for retrieving time zone data.
        /// </summary>
        /// <remarks>
        /// <para>Provides time zone information for locations around the world.</para>
        /// <para>Used for:</para>
        /// <para>- Determining venue local time</para>
        /// <para>- Scheduling special events in the correct time zone</para>
        /// <para>- Converting UTC times to the local time of a venue</para>
        /// </remarks>
        public MapsTimeZoneClient TimeZonesClient { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureMapsApiService"/> class.
        /// </summary>
        /// <param name="azureMapsKeyCredential">The Azure Maps API key credential for authentication.</param>
        /// <remarks>
        /// <para>This constructor initializes all three Azure Maps clients with the provided credential.</para>
        /// <para>The credential is obtained from application configuration and securely managed.</para>
        /// </remarks>
        public AzureMapsApiService(AzureKeyCredential azureMapsKeyCredential)
        {
            GeolocationClient = new MapsGeolocationClient(azureMapsKeyCredential);
            SearchClient = new MapsSearchClient(azureMapsKeyCredential);
            TimeZonesClient = new MapsTimeZoneClient(azureMapsKeyCredential);
        }
    }
}
