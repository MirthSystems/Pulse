namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the required and optional fields for creating a new venue entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>It requires nested address and operating schedule information to create a complete venue profile.</para>
    /// </remarks>
    public class CreateVenueRequest
    {
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
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals."</para>
        /// <para>- "Upscale cocktail bar with rotating seasonal menu."</para>
        /// </remarks>
        [StringLength(500)]
        public string? Description { get; set; }

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
        public string? PhoneNumber { get; set; }

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
        public string? Website { get; set; }

        /// <summary>
        /// Gets or sets the venue's email address.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

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
        public string? ProfileImage { get; set; }

        /// <summary>
        /// Gets or sets the address information for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This is a nested object containing all address components.</para>
        /// <para>The address will be geocoded to determine the venue's geographic coordinates.</para>
        /// </remarks>
        [Required]
        public AddressRequest Address { get; set; } = new AddressRequest();

        /// <summary>
        /// Gets or sets the collection of operating schedule entries for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection should include operating hours for each day of the week.</para>
        /// <para>Typically, there should be seven entries, one for each day of the week.</para>
        /// <para>Each entry specifies whether the venue is open on that day and, if so, the opening and closing times.</para>
        /// </remarks>
        [Required]
        [MinLength(1, ErrorMessage = "At least one operating schedule must be provided.")]
        public ICollection<OperatingHours> HoursOfOperation { get; set; } = new List<OperatingHours>();
    }
}
