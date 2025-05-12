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
    public class VenueItemExtended : VenueItem
    {
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
        /// Gets or sets the collection of operating schedule items for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection typically contains seven items, one for each day of the week.</para>
        /// <para>Each item describes the opening and closing times for a specific day or indicates if the venue is closed on that day.</para>
        /// <para>The collection is ordered by day of week, starting from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        [Required]
        public ICollection<OperatingScheduleItem> BusinessHours { get; set; } = new List<OperatingScheduleItem>();

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
