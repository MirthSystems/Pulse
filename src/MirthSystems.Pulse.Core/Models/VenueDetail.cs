namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents detailed information about a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model provides comprehensive information about a venue for detailed display.</para>
    /// <para>It includes all fields needed for a venue's detail page, including location and contact information.</para>
    /// <para>Used for venue detail pages and venue management interfaces.</para>
    /// </remarks>
    public class VenueDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a detailed description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers featuring local breweries. Our happy hour runs daily from 4-6pm."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals since 1985. Famous for our all-day breakfast."</para>
        /// </remarks>
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's phone number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// </remarks>
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's website URL.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://downtownmusichall.com"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's email address.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to the venue's profile image.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the street address of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St"</para>
        /// <para>- "456 Broadway"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any secondary address information.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200"</para>
        /// <para>- "Floor 2"</para>
        /// </remarks>
        [StringLength(255)]
        public string SecondaryAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city or locality where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "San Francisco"</para>
        /// <para>- "Brooklyn"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois"</para>
        /// <para>- "California"</para>
        /// <para>- "New York"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal or ZIP code of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "60601" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// </remarks>
        [Required]
        [StringLength(20)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the latitude coordinate of the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 41.8781 (Chicago)</para>
        /// <para>- 37.7749 (San Francisco)</para>
        /// <para>- 40.7128 (New York)</para>
        /// </remarks>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- -87.6298 (Chicago)</para>
        /// <para>- -122.4194 (San Francisco)</para>
        /// <para>- -74.0060 (New York)</para>
        /// </remarks>
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the collection of operating schedule items for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection typically contains seven items, one for each day of the week.</para>
        /// <para>Each item describes the opening and closing times for a specific day or indicates if the venue is closed on that day.</para>
        /// <para>The collection is ordered by day of week, starting from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        [Required]
        public ICollection<OperatingScheduleListItem> BusinessHours { get; set; } = new List<OperatingScheduleListItem>();

        /// <summary>
        /// Gets or sets the timestamp when the venue was created.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-01-01T08:00:00Z" for a venue created on January 1, 2023.</para>
        /// </remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the venue was last updated, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-02-15T10:00:00Z" for a venue updated on February 15, 2023.</para>
        /// </remarks>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
