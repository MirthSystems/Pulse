namespace MirthSystems.Pulse.Core.Interfaces
{
    using Azure.Maps.Geolocation;
    using Azure.Maps.Search;
    using Azure.Maps.TimeZones;

    /// <summary>
    /// Interface for accessing Azure Maps services for geospatial functionality.
    /// </summary>
    /// <remarks>
    /// <para>This interface provides access to Azure Maps API clients for various location and mapping services:</para>
    /// <para>- Geolocation services for IP-based location detection</para>
    /// <para>- Search services for geocoding and place search</para>
    /// <para>- Time Zone services for time zone information by location</para>
    /// <para>Implementation details and authentication are abstracted behind this interface.</para>
    /// </remarks>
    public interface IAzureMapsApiService
    {
        /// <summary>
        /// Gets the Azure Maps Geolocation client for IP-based and device location services.
        /// </summary>
        /// <remarks>
        /// <para>Used to determine a user's location from their IP address or device signals.</para>
        /// <para>Example: Automatic region detection when user hasn't provided explicit location.</para>
        /// </remarks>
        MapsGeolocationClient GeolocationClient { get; }

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
        MapsSearchClient SearchClient { get; }

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
        MapsTimeZoneClient TimeZonesClient { get; }
    }
}
