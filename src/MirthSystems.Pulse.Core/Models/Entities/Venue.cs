namespace MirthSystems.Pulse.Core.Models.Entities
{
    using System.Net;

    using NodaTime;

    /// <summary>
    /// Represents a venue entity within the Pulse platform.
    /// </summary>
    /// <remarks>
    /// <para>Venues are physical establishments such as bars, restaurants, cafes, or clubs that offer specials and events to users.</para>
    /// <para>Each venue can have multiple assigned users with different roles (owners, managers) through the VenueUser entity.</para>
    /// </remarks>
    public class Venue
    {
        /// <summary>
        /// Gets or sets the unique identifier for the venue.
        /// </summary>
        /// <remarks>
        /// This is the primary key for the venue entity in the database.
        /// </remarks>
        public long Id { get; set; }

        /// <summary>
        /// This required field provides the primary identifier for the venue as displayed to users.
        /// </summary>
        /// <remarks>
        /// <para>Examples of venue names include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        public required string Name { get; set; }

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
        public string? ProfileImage { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the Address entity.
        /// </summary>
        /// <remarks>
        /// <para>This represents the physical address where the venue is located.</para>
        /// <para>This is a required field and is used to establish the relationship with the Address entity.</para>
        /// </remarks>
        public long AddressId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the venue was created.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-01-01T08:00:00Z" for a venue created on January 1, 2023.</para>
        /// </remarks>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the venue.
        /// </summary>
        /// <remarks>
        /// <para>Example: "auth0|12345" for the user who created the venue.</para>
        /// </remarks>
        public required string CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the venue was last updated, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-02-15T10:00:00Z" for a venue updated on February 15, 2023.</para>
        /// </remarks>
        public Instant? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who last updated the venue, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "auth0|12345" for the user who last updated the venue.</para>
        /// </remarks>
        public string? UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the venue has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// <para>Default is false. When true, the venue is considered deleted but remains in the database for auditing purposes.</para>
        /// </remarks>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the timestamp when the venue was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-03-01T12:00:00Z" for a venue deleted on March 1, 2023.</para>
        /// </remarks>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted the venue, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "auth0|12345" for the user who performed the deletion.</para>
        /// </remarks>
        public string? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the associated address.
        /// </summary>
        /// <remarks>
        /// <para>Example: Address with Line1 "123 Main St".</para>
        /// </remarks>
        public required virtual Address Address { get; set; }

        /// <summary>
        /// Gets or sets the collection of business hours for this venue.
        /// </summary>
        /// <remarks>
        /// <para>Example: Operating schedules for each day of the week.</para>
        /// </remarks>
        public virtual ICollection<OperatingSchedule> BusinessHours { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of specials associated with this venue.
        /// </summary>
        /// <remarks>
        /// <para>Example: Specials like "Half-Price Wings Happy Hour".</para>
        /// </remarks>
        public virtual ICollection<Special> Specials { get; set; } = [];
    }
}
