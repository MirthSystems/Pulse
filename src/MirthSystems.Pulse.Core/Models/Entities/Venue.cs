namespace MirthSystems.Pulse.Core.Models.Entities
{
    using System.Net;

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
        /// Gets or sets the navigation property to the associated Address.
        /// </summary>
        /// <remarks>
        /// <para>This navigation property links to the physical address of the venue.</para>
        /// <para>It provides access to the address information such as street, city, and geographic coordinates.</para>
        /// </remarks>
        public required virtual Address Address { get; set; }

        /// <summary>
        /// Gets or sets the collection of VenueUser associations for this venue.
        /// </summary>
        /// <remarks>
        /// <para>This navigation property represents the users who have a relationship with this venue.</para>
        /// <para>Each VenueUser defines a user's role(s) within the venue context.</para>
        /// <para>Common roles include Venue Owner and Venue Manager, which determine the user's permissions.</para>
        /// </remarks>
        public virtual ICollection<VenueUser> VenueUsers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of operating schedules for this venue.
        /// </summary>
        /// <remarks>
        /// <para>This navigation property represents the business hours for each day of the week.</para>
        /// <para>A venue typically has 7 operating schedule entries, one for each day of the week.</para>
        /// <para>Operating schedules define when the venue is open, closed, or has special hours.</para>
        /// </remarks>
        public virtual ICollection<OperatingSchedule> BusinessHours { get; set; } = [];
    }
}
