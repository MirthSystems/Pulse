namespace MirthSystems.Pulse.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class VenueDetail
    {
        /// <summary>
        /// The unique identifier for the venue
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The name of the venue
        /// </summary>
        /// <remarks>e.g. The Rusty Anchor Pub</remarks>
        public required string Name { get; set; }

        /// <summary>
        /// Description of the venue and its offerings
        /// </summary>
        /// <remarks>e.g. A cozy pub with live music and craft beers.</remarks>
        public string? Description { get; set; }

        /// <summary>
        /// Contact phone number for the venue
        /// </summary>
        /// <remarks>e.g. +1 (555) 123-4567</remarks>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Website URL for the venue
        /// </summary>
        /// <remarks>e.g. https://www.rustyanchorpub.com</remarks>
        public string? Website { get; set; }

        /// <summary>
        /// Contact email address for the venue
        /// </summary>
        /// <remarks>e.g. info@rustyanchorpub.com</remarks>
        public string? Email { get; set; }

        /// <summary>
        /// URL to the venue's profile image
        /// </summary>
        /// <remarks>e.g. https://cdn.pulse.com/venues/123456/profile.jpg</remarks>
        public string? ProfileImage { get; set; }

        /// <summary>
        /// The street address line 1 of the venue
        /// </summary>
        /// <remarks>e.g. 123 Main Street</remarks>
        public required string StreetAddress { get; set; }

        /// <summary>
        /// The street address line 2 of the venue
        /// </summary>
        /// <remarks>e.g. Suite 200</remarks>
        public string? SecondaryAddress { get; set; }

        /// <summary>
        /// The city or town of the venue
        /// </summary>
        /// <remarks>e.g. Springfield</remarks>
        public required string Locality { get; set; }

        /// <summary>
        /// The state, province, or region of the venue
        /// </summary>
        /// <remarks>e.g. Illinois</remarks>
        public required string Region { get; set; }

        /// <summary>
        /// The postal or ZIP code of the venue
        /// </summary>
        /// <remarks>e.g. 62701</remarks>
        public required string Postcode { get; set; }

        /// <summary>
        /// The country of the venue
        /// </summary>
        /// <remarks>e.g. United States</remarks>
        public required string Country { get; set; }

        /// <summary>
        /// The venue's latitude coordinate
        /// </summary>
        /// <remarks>e.g. 41.8781</remarks>
        public double? Latitude { get; set; }

        /// <summary>
        /// The venue's longitude coordinate
        /// </summary>
        /// <remarks>e.g. -87.6298</remarks>
        public double? Longitude { get; set; }

        /// <summary>
        /// The operating hours for each day of the week
        /// </summary>
        public required ICollection<OperatingScheduleListItem> BusinessHours { get; set; }

        /// <summary>
        /// When the venue was created
        /// </summary>
        /// <remarks>e.g. 2025-01-01T08:00:00Z</remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// When the venue was last updated, if applicable
        /// </summary>
        /// <remarks>e.g. 2025-02-15T10:00:00Z</remarks>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
