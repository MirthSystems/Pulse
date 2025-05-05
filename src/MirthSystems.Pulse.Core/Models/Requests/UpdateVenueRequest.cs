namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing venue
    /// </summary>
    public class UpdateVenueRequest
    {
        /// <summary>
        /// The name of the venue
        /// </summary>
        /// <remarks>e.g. The Rusty Anchor Pub</remarks>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }

        /// <summary>
        /// Description of the venue and its offerings
        /// </summary>
        /// <remarks>e.g. A cozy pub with live music and craft beers.</remarks>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Contact phone number for the venue
        /// </summary>
        /// <remarks>e.g. +1 (555) 123-4567</remarks>
        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Website URL for the venue
        /// </summary>
        /// <remarks>e.g. https://www.rustyanchorpub.com</remarks>
        [Url]
        [StringLength(255)]
        public string? Website { get; set; }

        /// <summary>
        /// Contact email address for the venue
        /// </summary>
        /// <remarks>e.g. info@rustyanchorpub.com</remarks>
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// URL to the venue's profile image
        /// </summary>
        /// <remarks>e.g. https://cdn.pulse.com/venues/123456/profile.jpg</remarks>
        [Url]
        [StringLength(255)]
        public string? ProfileImage { get; set; }

        /// <summary>
        /// The physical address of the venue
        /// </summary>
        [Required]
        public required UpdateAddressRequest Address { get; set; }
    }
}
