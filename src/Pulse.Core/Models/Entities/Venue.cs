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
        /// This required field helps classify and filter venues for users. (e.g., "pub", "bar", "restaurant")
        /// </summary>
        /// <remarks>
        /// <para>Examples of venue categories include:</para>
        /// <para>- "pub"</para>
        /// <para>- "restaurant"</para>
        /// <para>- "comedy_club"</para>
        /// <para>- "winery"</para>
        /// </remarks>
        public string Category { get; set; } = null!;

        /// <summary>
        /// This required field typically includes the street number and name.
        /// </summary>
        /// <remarks>
        /// <para>Examples of address line 1 include:</para>
        /// <para>- "123 Main St" (USA)</para>
        /// <para>- "10 Downing Street" (UK)</para>
        /// </remarks>
        public string AddressLine1 { get; set; } = null!;

        /// <summary>
        /// This optional field can include additional address details like suite or apartment numbers.
        /// </summary>
        /// <remarks>
        /// <para>Examples of address line 2 include:</para>
        /// <para>- "Suite 200" (USA)</para>
        /// <para>- "Flat 3" (UK)</para>
        /// </remarks>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// The city or locality where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples of cities include:</para>
        /// <para>- "Springfield" (USA)</para>
        /// <para>- "Toronto" (Canada)</para>
        /// </remarks>
        public string City { get; set; } = null!;

        /// <summary>
        /// The administrative region where the venue is located.
        /// This required field can represent a state, province, territory, or other regional division depending on the country.
        /// For the MVP, this focuses on US states and territories, but it is flexible for international use.
        /// </summary>
        /// <remarks>
        /// <para>Examples of regions include:</para>
        /// <para>- "Illinois" (USA)</para>
        /// <para>- "Puerto Rico" (US Territory)</para>
        /// <para>- "Ontario" (Canada)</para>
        /// <para>- "Queensland" (Australia)</para>
        /// </remarks>
        public string Region { get; set; } = null!;

        /// <summary>
        /// The postal or ZIP code of the venue's location.
        /// This required field is part of the venue's address and supports various international formats.
        /// </summary>
        /// <remarks>
        /// <para>Examples of postal codes include:</para>
        /// <para>- "62701" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// </remarks>
        public string ZipCode { get; set; } = null!;

        /// <summary>
        /// The country where the venue is located.
        /// This required field is part of the venue's address and helps determine address formatting and timezone derivation.
        /// </summary>
        /// <remarks>
        /// <para>Examples of countries include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// </remarks>
        public string Country { get; set; } = null!;

        /// <summary>
        /// The ISO 3166-1 alpha-2 country code for the venue's location.
        /// This optional field standardizes country identification and supports geocoding or API integrations.
        /// </summary>
        /// <remarks>
        /// <para>Examples of country codes include:</para>
        /// <para>- "US" (United States)</para>
        /// <para>- "CA" (Canada)</para>
        /// <para>- "GB" (United Kingdom)</para>
        /// </remarks>
        public string? CountryCode { get; set; }

        /// <summary>
        /// The geographical coordinates (latitude and longitude) of the venue.
        /// This optional field, leveraging TIGER data for US venues, is used for mapping and deriving the venue's timezone on the frontend.
        /// </summary>
        /// <remarks>
        /// <para>Examples of locations include:</para>
        /// <para>- Point(-87.6298, 41.8781) for Chicago, IL, USA</para>
        /// <para>- Point(-0.1276, 51.5074) for London, UK</para>
        /// </remarks>
        public Point? Location { get; set; }

        /// <summary>
        /// The phone number of the venue.
        /// This optional field allows users to contact the venue directly and supports international formats.
        /// </summary>
        /// <remarks>
        /// <para>Examples of phone numbers include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// </remarks>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The website URL of the venue.
        /// This optional field provides a link to the venue's online presence.
        /// </summary>
        /// <remarks>
        /// <para>Examples of websites include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://www.downtownmusichall.com"</para>
        /// </remarks>
        public string? Website { get; set; }

        /// <summary>
        /// The email address of the venue.
        /// This optional field allows for direct email contact.
        /// </summary>
        /// <remarks>
        /// <para>Examples of email addresses include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        public string? Email { get; set; }

        /// <summary>
        /// The list of specials associated with the venue.
        /// This navigation property links to all specials hosted by the venue.
        /// </summary>
        public List<Special> Specials { get; set; } = [];
    }
}
