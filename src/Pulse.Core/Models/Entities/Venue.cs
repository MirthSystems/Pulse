namespace Pulse.Core.Models.Entities
{

    using NetTopologySuite.Geometries;


    /// <summary>
    /// Represents a venue hosting specials, such as bars, restaurants, or entertainment venues.
    /// This entity captures essential details about the venue, including its location, contact information, and associated specials.
    /// Designed to handle addresses from all countries, with fields flexible enough to accommodate various international address formats.
    /// </summary>
    public class Venue : EntityBase
    {
        public long Id { get; set; }

        public int VenueTypeId { get; set; }

        /// <summary>
        /// This required field provides the primary identifier for the venue as displayed to users.
        /// </summary>
        /// <remarks>
        /// <para>Examples of venue names include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        public string Name { get; set; } = null!;

        /// <summary>
        /// This optional field provides an overview of the venue's offerings or unique features.
        /// </summary>
        /// <remarks>
        /// <para>Examples of descriptions include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals."</para>
        /// <para>- "Upscale cocktail bar with rotating seasonal menu and expert mixologists."</para>
        /// </remarks>
        public string? Description { get; set; }

        /// <summary>
        /// This optional field allows users to contact the venue directly and supports international formats.
        /// </summary>
        /// <remarks>
        /// <para>Examples of phone numbers include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// <para>- "+61 2 9876 5432" (Australia)</para>
        /// <para>Recommended format is the E.164 international standard: country code + number without spaces.</para>
        /// </remarks>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// This optional field provides a link to the venue's online presence.
        /// </summary>
        /// <remarks>
        /// <para>Examples of websites include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://downtownmusichall.com"</para>
        /// <para>- "http://cafemilano.biz"</para>
        /// <para>Always include the full URL including the protocol (http:// or https://).</para>
        /// </remarks>
        public string? Website { get; set; }

        /// <summary>
        /// This optional field allows for direct email contact with the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples of email addresses include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// <para>- "reservations@cafemilano.biz"</para>
        /// </remarks>
        public string? Email { get; set; }

        /// <summary>
        /// URL to the venue's profile image (square format).
        /// </summary>
        /// <remarks>
        /// <para>Examples of profile image URLs include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// <para>Recommended image specs: 512x512 pixels, square aspect ratio, JPG or PNG format.</para>
        /// <para>Images should be optimized for web to keep file sizes under 200KB.</para>
        /// </remarks>
        public string? ImageLink { get; set; }

        /// <summary>
        /// This required field captures the primary address information, typically including the street number and name.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St" (USA)</para>
        /// <para>- "10 Downing Street" (UK)</para>
        /// <para>- "35 Rue du Faubourg Saint-Honoré" (France)</para>
        /// </remarks>
        public string AddressLine1 { get; set; } = null!;

        /// <summary>
        /// This optional field captures additional address details like suite or unit numbers.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200" (USA)</para>
        /// <para>- "Flat 3" (UK)</para>
        /// <para>- "Apartment 12B" (General)</para>
        /// </remarks>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// This optional field captures additional address information if needed for complex addresses.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Building C, Floor 3"</para>
        /// <para>- "Corner of Oak and Pine"</para>
        /// <para>- "Inside Central Mall"</para>
        /// </remarks>
        public string? AddressLine3 { get; set; }

        /// <summary>
        /// This optional field captures additional address information if needed for complex addresses.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Entrance on South Side"</para>
        /// <para>- "Near Transportation Hub"</para>
        /// <para>- "Industrial Park Zone B"</para>
        /// </remarks>
        public string? AddressLine4 { get; set; }

        /// <summary>
        /// The city, town, or locality where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Springfield" (USA)</para>
        /// <para>- "Toronto" (Canada)</para>
        /// <para>- "Manchester" (UK)</para>
        /// </remarks>
        public string Locality { get; set; } = null!;

        /// <summary>
        /// The administrative region where the venue is located.
        /// This required field can represent a state, province, territory, or other regional division depending on the country.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois" (USA)</para>
        /// <para>- "Ontario" (Canada)</para>
        /// <para>- "Queensland" (Australia)</para>
        /// <para>- "Bavaria" (Germany)</para>
        /// </remarks>
        public string Region { get; set; } = null!;

        /// <summary>
        /// The postal or ZIP code of the venue's location.
        /// This required field supports various international formats.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "62701" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// <para>- "2000" (Australia)</para>
        /// </remarks>
        public string Postcode { get; set; } = null!;

        /// <summary>
        /// The country where the venue is located.
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
        public string Country { get; set; } = null!;

        /// <summary>
        /// The geographical coordinates (latitude and longitude) of the venue.
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

        public virtual VenueType VenueType { get; set; } = null!;

        /// <summary>
        /// The list of specials associated with the venue.
        /// </summary>
        public virtual List<Special> Specials { get; set; } = [];

        /// <summary>
        /// The business hours for this venue, typically one schedule for each day of the week.
        /// </summary>
        /// <remarks>
        /// <para>Should include entries for all seven days of the week.</para>
        /// <para>For days when the venue is closed, set IsClosed = true.</para>
        /// <para>For venues open 24 hours, set TimeOfOpen to 00:00 and TimeOfClose to 23:59.</para>
        /// </remarks>
        public virtual List<OperatingSchedule> BusinessHours { get; set; } = [];
    }
}
