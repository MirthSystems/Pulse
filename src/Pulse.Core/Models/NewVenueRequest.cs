namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new venue
    /// </summary>
    public class NewVenueRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(255)]
        public string AddressLine1 { get; set; } = null!;

        [StringLength(255)]
        public string? AddressLine2 { get; set; }

        [StringLength(255)]
        public string? AddressLine3 { get; set; }

        [StringLength(255)]
        public string? AddressLine4 { get; set; }

        [Required]
        [StringLength(100)]
        public string Locality { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Region { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Postcode { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = null!;

        [StringLength(20)]
        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(255)]
        [Url]
        public string? Website { get; set; }

        [StringLength(512)]
        [Url]
        public string? ImageLink { get; set; }

        [Required]
        public int VenueTypeId { get; set; }
    }
}
