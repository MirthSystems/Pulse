namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;

    using NetTopologySuite.Geometries;

    /// <summary>
    /// Repository interface for address entities, extending the base repository with address-specific operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines address-specific data access operations beyond the basic CRUD operations.</para>
    /// <para>It provides methods for address standardization and location-based queries.</para>
    /// <para>Implementations handle the actual data access logic and database interactions.</para>
    /// </remarks>
    public interface IAddressRepository : IRepository<Address>
    {
        /// <summary>
        /// Standardizes an address string using PostgreSQL's address_standardizer extension.
        /// </summary>
        /// <param name="addressString">The address string to standardize.</param>
        /// <returns>A standardized address object with parsed components.</returns>
        /// <remarks>
        /// <para>This method uses PostgreSQL's address_standardizer extension to parse an address into its components.</para>
        /// <para>The standardization process follows USPS addressing guidelines.</para>
        /// <para>Components include street number, name, suffix, city, state, etc.</para>
        /// <para>This is useful for ensuring consistent address formats and for geocoding preparation.</para>
        /// <para>Example input: "123 Main St, Chicago, IL 60601"</para>
        /// <para>Example output components: house_num="123", name="MAIN", suftype="ST", city="CHICAGO", state="IL", postcode="60601"</para>
        /// </remarks>
        Task<StandardizedAddress> StandardizeAddressAsync(string addressString);

        /// <summary>
        /// Finds the nearest address to a given geographic point.
        /// </summary>
        /// <param name="location">The geographic point to search from.</param>
        /// <param name="maxDistanceInMeters">The maximum search distance in meters.</param>
        /// <returns>The nearest address within the specified maximum distance, or null if none found.</returns>
        /// <remarks>
        /// <para>This method performs a spatial query to find the nearest address to a specified location.</para>
        /// <para>The search is limited by the maximum distance parameter to avoid returning very distant matches.</para>
        /// <para>Results are ordered by distance (closest first).</para>
        /// <para>This is useful for reverse geocoding (converting coordinates to an address) and for finding nearby venues.</para>
        /// <para>Example usage: Find the nearest venue to a user's current location.</para>
        /// </remarks>
        Task<Address?> FindNearestAddressAsync(Point location, double maxDistanceInMeters);
    }
}
