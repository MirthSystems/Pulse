namespace Pulse.Core.Models.Entities
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using NetTopologySuite.Geometries;

    using NodaTime;

    using Pulse.Core.Enums;


    /// <summary>
    /// Represents a venue hosting specials, such as bars, restaurants, or entertainment venues.
    /// This entity captures essential details about the venue, including its location, contact information, and associated specials.
    /// Designed to handle addresses from all countries, with fields flexible enough to accommodate various international address formats.
    /// </summary>
    public class Venue
    {
        public int Id { get; set; }

        /// <summary>
        /// This required field provides the primary identifier for the venue as displayed to users.
        /// </summary>
        /// <remarks>
        /// <para>Examples of venue names include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// </remarks>
        public string Name { get; set; } = null!;

        /// <summary>
        /// This optional field provides an overview of the venue's offerings or unique features.
        /// </summary>
        /// <remarks>
        /// <para>Examples of descriptions include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials."</para>
        /// </remarks>
        public string? Description { get; set; }

        /// <summary>
        /// This optional field allows users to contact the venue directly and supports international formats.
        /// </summary>
        /// <remarks>
        /// <para>Examples of phone numbers include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// </remarks>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// This optional field provides a link to the venue's online presence.
        /// </summary>
        /// <remarks>
        /// <para>Examples of websites include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://www.downtownmusichall.com"</para>
        /// </remarks>
        public string? Website { get; set; }

        /// <summary>
        /// This optional field allows for direct email contact with the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples of email addresses include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        public string? Email { get; set; }

        /// <summary>
        /// This required field captures the primary address information, typically including the street number and name.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St" (USA)</para>
        /// <para>- "10 Downing Street" (UK)</para>
        /// </remarks>
        public string AddressLine1 { get; set; } = null!;

        /// <summary>
        /// This optional field captures additional address details like suite or unit numbers.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200" (USA)</para>
        /// <para>- "Flat 3" (UK)</para>
        /// </remarks>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// This optional field captures additional address information if needed for complex addresses.
        /// </summary>
        public string? AddressLine3 { get; set; }

        /// <summary>
        /// This optional field captures additional address information if needed for complex addresses.
        /// </summary>
        public string? AddressLine4 { get; set; }

        /// <summary>
        /// The city, town, or locality where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Springfield" (USA)</para>
        /// <para>- "Toronto" (Canada)</para>
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
        /// </remarks>
        public string Country { get; set; } = null!;

        /// <summary>
        /// The geographical coordinates (latitude and longitude) of the venue.
        /// Used for mapping and location-based features.
        /// </summary>
        public Point? Location { get; set; }

        /// <summary>
        /// The list of specials associated with the venue.
        /// </summary>
        public List<Special> Specials { get; set; } = [];
    }
}
