namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VenueListItem
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Locality { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
