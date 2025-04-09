namespace Pulse.Core.Models.Entities
{
    using NetTopologySuite.Geometries;

    using NodaTime;

    /// <summary>
    /// Represents a user of the Pulse application
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// Primary key identifier for this user in our database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User identifier from the external identity provider (Auth0)
        /// </summary>
        public string ExternalId { get; set; } = null!;

        /// <summary>
        /// When this user account was created
        /// </summary>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// When this user last logged in
        /// </summary>
        public Instant? LastLoginAt { get; set; }

        /// <summary>
        /// The string representation of the user's default search location (e.g., "Chicago, IL")
        /// </summary>
        /// <remarks>
        /// <para>Storing the string representation alongside coordinates allows displaying it in the UI 
        /// without reverse geocoding, improving performance and reducing API calls.</para>
        /// <para>This preserves the original format of the location as entered or selected by the user.</para>
        /// </remarks>
        public string? DefaultSearchLocationString { get; set; }

        /// <summary>
        /// User's default search location as geographic coordinates
        /// </summary>
        /// <remarks>
        /// <para>This uses NetTopologySuite.Geometries.Point to store geographic coordinates.</para>
        /// <para>Example of creating a Point:</para>
        /// <para>- new Point(longitude, latitude) { SRID = 4326 }</para>
        /// <para>- new Point(-87.6298, 41.8781) { SRID = 4326 } // Chicago</para>
        /// <para>Note: SRID 4326 is the spatial reference system for WGS 84, the standard for GPS.</para>
        /// <para>Important: Point constructor takes (longitude, latitude) in that order, not (latitude, longitude).</para>
        /// </remarks>
        public Point? DefaultSearchLocation { get; set; }

        /// <summary>
        /// User's preferred search radius in miles
        /// </summary>
        public double DefaultSearchRadius { get; set; } = 5.0;

        /// <summary>
        /// Whether the user has opted in to location services
        /// </summary>
        public bool OptedInToLocationServices { get; set; } = false;

        /// <summary>
        /// Whether the user account is active in our application
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Posts created by this user
        /// </summary>
        public virtual List<Post> Posts { get; set; } = [];
    }
}
