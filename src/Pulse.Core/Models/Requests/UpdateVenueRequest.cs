namespace Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateVenueRequest
    {
        [Required]
        public int VenueTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        [Url]
        public string? Website { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(512)]
        [Url]
        public string? ImageLink { get; set; }

        [Required]
        [StringLength(255)]
        public required string AddressLine1 { get; set; }

        [StringLength(255)]
        public string? AddressLine2 { get; set; }

        [StringLength(255)]
        public string? AddressLine3 { get; set; }

        [StringLength(255)]
        public string? AddressLine4 { get; set; }

        [Required]
        [StringLength(100)]
        public required string Locality { get; set; }

        [Required]
        [StringLength(100)]
        public required string Region { get; set; }

        [Required]
        [StringLength(20)]
        public required string Postcode { get; set; }

        [Required]
        [StringLength(100)]
        public required string Country { get; set; }

        public List<OperatingHoursRequest> OperatingHours { get; set; } = new List<OperatingHoursRequest>();
    }
}
