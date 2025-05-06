namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VenueDetail
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Url]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Url]
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string StreetAddress { get; set; } = string.Empty;

        [StringLength(255)]
        public string SecondaryAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Locality { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Postcode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Required]
        public ICollection<OperatingScheduleListItem> BusinessHours { get; set; } = new List<OperatingScheduleListItem>();

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
