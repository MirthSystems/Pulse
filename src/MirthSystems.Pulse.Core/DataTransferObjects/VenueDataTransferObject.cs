namespace MirthSystems.Pulse.Core.DataTransferObjects
{
    using System;
    using System.Collections.Generic;

    public class VenueDataTransferObject
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
        /// The physical address of the venue
        /// </summary>
        public AddressDataTransferObject? Address { get; set; }

        /// <summary>
        /// The operating hours for each day of the week
        /// </summary>
        public IEnumerable<OperatingScheduleDataTransferObject>? BusinessHours { get; set; }

        /// <summary>
        /// When the venue was created
        /// </summary>
        /// <remarks>e.g. 2025-01-01T08:00:00Z</remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The user ID of who created the venue
        /// </summary>
        /// <remarks>e.g. auth0|12345</remarks>
        public required string CreatedByUserId { get; set; }

        /// <summary>
        /// When the venue was last updated, if applicable
        /// </summary>
        /// <remarks>e.g. 2025-02-15T10:00:00Z</remarks>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// The user ID of who last updated the venue, if applicable
        /// </summary>
        /// <remarks>e.g. auth0|12345</remarks>
        public string? UpdatedByUserId { get; set; }
    }
}
