namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NetTopologySuite.Geometries;

    /// <summary>
    /// Represents a physical address within the Pulse platform.
    /// </summary>
    /// <remarks>
    /// <para>Addresses are used to store location information for venues and other geographic entities.</para>
    /// <para>Each address includes standard addressing components and can optionally include geographic coordinates.</para>
    /// </remarks>
    public class Address
    {
        /// <summary>
        /// Gets or sets the unique identifier for the address.
        /// </summary>
        /// <remarks>
        /// This is the primary key for the address entity in the database.
        /// </remarks>
        public long Id { get; set; }

        /// <summary>
        /// This required field captures the primary address information, typically including the street number and name.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St" (USA)</para>
        /// <para>- "10 Downing Street" (UK)</para>
        /// <para>- "35 Rue du Faubourg Saint-Honoré" (France)</para>
        /// </remarks>
        public required string StreetAddress { get; set; }

        /// <summary>
        /// This optional field captures additional address details like suite or unit numbers.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200" (USA)</para>
        /// <para>- "Flat 3" (UK)</para>
        /// <para>- "Apartment 12B" (General)</para>
        /// </remarks>
        public string? SecondaryAddress { get; set; }

        /// <summary>
        /// The city, town, or locality where the address is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Springfield" (USA)</para>
        /// <para>- "Toronto" (Canada)</para>
        /// <para>- "Manchester" (UK)</para>
        /// </remarks>
        public required string Locality { get; set; }

        /// <summary>
        /// The administrative region where the address is located.
        /// This required field can represent a state, province, territory, or other regional division depending on the country.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois" (USA)</para>
        /// <para>- "Ontario" (Canada)</para>
        /// <para>- "Queensland" (Australia)</para>
        /// <para>- "Bavaria" (Germany)</para>
        /// </remarks>
        public required string Region { get; set; }

        /// <summary>
        /// The postal or ZIP code of the address's location.
        /// This required field supports various international formats.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "62701" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// <para>- "2000" (Australia)</para>
        /// </remarks>
        public required string Postcode { get; set; }

        /// <summary>
        /// The country where the address is located.
        /// This required field helps determine address formatting and timezone derivation.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// <para>- "Australia"</para>
        /// <para>Use the full country name rather than abbreviations or codes.</para>
        /// </remarks>
        public required string Country { get; set; }

        /// <summary>
        /// The geographical coordinates (latitude and longitude) of the address.
        /// Used for mapping and location-based features.
        /// </summary>
        /// <remarks>
        /// <para>This uses NetTopologySuite.Geometries.Point to store geographic coordinates.</para>
        /// <para>Example of creating a Point:</para>
        /// <para>- new Point(longitude, latitude) { SRID = 4326 }</para>
        /// <para>- new Point(-87.6298, 41.8781) { SRID = 4326 } // Chicago</para>
        /// <para>- new Point(-0.1278, 51.5074) { SRID = 4326 } // London</para>
        /// <para>Note: SRID 4326 is the spatial reference system for WGS 84, the standard for GPS.</para>
        /// <para>Important: Point constructor takes (longitude, latitude) in that order, not (latitude, longitude).</para>
        /// </remarks>
        public Point? Location { get; set; }

        public virtual Venue? Venue { get; set; }
    }
}
