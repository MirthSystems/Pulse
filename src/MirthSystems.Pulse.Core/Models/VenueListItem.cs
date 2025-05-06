namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a summary view of a venue for display in lists and search results.
    /// </summary>
    /// <remarks>
    /// <para>This model provides essential information about a venue for preview purposes.</para>
    /// <para>It includes only the most important fields needed for venue listings.</para>
    /// <para>Used for venue listings, search results, and map markers.</para>
    /// </remarks>
    public class VenueListItem
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
        /// Gets or sets a brief description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals."</para>
        /// <para>- "Upscale cocktail bar with rotating seasonal menu."</para>
        /// </remarks>
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

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
        /// Gets or sets the URL to the venue's profile image.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// </remarks>
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

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
    }
}
