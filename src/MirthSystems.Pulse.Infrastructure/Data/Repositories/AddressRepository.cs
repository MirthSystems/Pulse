namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;

    using NetTopologySuite.Geometries;

    /// <summary>
    /// Repository for managing address entities in the database.
    /// </summary>
    /// <remarks>
    /// <para>This repository extends the base repository with address-specific query methods.</para>
    /// <para>It handles address-related data access operations including:</para>
    /// <para>- Standardizing address formats using PostgreSQL's address_standardizer extension</para>
    /// <para>- Finding addresses by proximity to a geographic point</para>
    /// </remarks>
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks>
        /// Passes the database context to the base repository constructor.
        /// </remarks>
        public AddressRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Standardizes an address string using PostgreSQL's address_standardizer extension.
        /// </summary>
        /// <param name="addressString">The address string to standardize.</param>
        /// <returns>A standardized address object with parsed components.</returns>
        /// <remarks>
        /// <para>This method uses PostgreSQL's address_standardizer extension to parse an address string into its components.</para>
        /// <para>The standardization process follows USPS addressing standards.</para>
        /// <para>If the address cannot be standardized, an empty StandardizedAddress object is returned.</para>
        /// <para>Example input: "123 Main St, Anytown, CA 12345"</para>
        /// <para>Example output components: house_num="123", name="Main", suftype="St", city="Anytown", state="CA", postcode="12345"</para>
        /// </remarks>
        public async Task<StandardizedAddress> StandardizeAddressAsync(string addressString)
        {
            var result = await _context.Database.SqlQueryRaw<StandardizedAddress>(
                @"SELECT building, house_num, predir, qual, pretype, name, suftype, sufdir, ruralroute, extra, city, state, country, postcode, box, unit   
                     FROM standardize_address('us_lex', 'us_gaz', 'us_rules', @addressString)",
                new SqlParameter("@addressString", addressString))
                .FirstOrDefaultAsync();

            return result ?? new StandardizedAddress();
        }

        /// <summary>
        /// Finds the nearest address to a given geographic point within a maximum distance.
        /// </summary>
        /// <param name="location">The geographic point to search from.</param>
        /// <param name="maxDistanceInMeters">The maximum search distance in meters.</param>
        /// <returns>The nearest address within the specified maximum distance, or null if none found.</returns>
        /// <remarks>
        /// <para>This method performs a spatial query using PostGIS capabilities to find the nearest address.</para>
        /// <para>Results are ordered by distance from the specified location (closest first).</para>
        /// <para>Only addresses within the specified maximum distance are considered.</para>
        /// <para>The distance is calculated using the geodesic distance formula appropriate for geographic coordinates.</para>
        /// </remarks>
        public async Task<Address?> FindNearestAddressAsync(Point location, double maxDistanceInMeters)
        {
            return await _context.Addresses
                .OrderBy(a => a.Location.Distance(location))
                .FirstOrDefaultAsync(a => a.Location.Distance(location) <= maxDistanceInMeters);
        }
    }
}
