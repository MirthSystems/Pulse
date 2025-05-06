namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating an address
    /// </summary>
    public class CreateAddressRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string StreetAddress { get; set; } = string.Empty;

        [StringLength(50)]
        public string SecondaryAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Locality { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Postcode { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;
    }
}
