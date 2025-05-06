namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the fields that can be modified when updating a venue entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Unlike CreateVenueRequest, this does not include operating schedules as those are updated separately.</para>
    /// </remarks>
    public class UpdateVenueRequest
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
        [StringLength(100)]
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
        /// Gets or sets the updated address information for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This is a nested object containing all address components.</para>
        /// <para>The updated address will be geocoded to determine the venue's new geographic coordinates.</para>
        /// <para>If the address changes significantly, this may affect location-based searches and nearby venue results.</para>
        /// </remarks>
        [Required]
        public UpdateAddressRequest Address { get; set; } = new UpdateAddressRequest();
    }
}
