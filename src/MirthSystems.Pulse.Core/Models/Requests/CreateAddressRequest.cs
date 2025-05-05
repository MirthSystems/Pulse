namespace MirthSystems.Pulse.Core.Models.Requests
// No changes needed; all properties already have appropriate data annotations.
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating an address
    /// </summary>
    public class CreateAddressRequest
    {
        /// <summary>
        /// The primary street address information
        /// </summary>
        /// <remarks>e.g. 123 Main Street</remarks>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string StreetAddress { get; set; }

        /// <summary>
        /// Additional address details like unit numbers
        /// </summary>
        /// <remarks>e.g. Suite 200</remarks>
        [StringLength(50)]
        public string? SecondaryAddress { get; set; }

        /// <summary>
        /// The city or town
        /// </summary>
        /// <remarks>e.g. Springfield</remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Locality { get; set; }

        /// <summary>
        /// The state, province, or region
        /// </summary>
        /// <remarks>e.g. Illinois</remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Region { get; set; }

        /// <summary>
        /// The postal or ZIP code
        /// </summary>
        /// <remarks>e.g. 62701</remarks>
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string Postcode { get; set; }

        /// <summary>
        /// The country
        /// </summary>
        /// <remarks>e.g. United States</remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Country { get; set; }
    }
}
