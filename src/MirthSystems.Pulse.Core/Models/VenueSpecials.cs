namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    public class VenueSpecials
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        [Required]
        public ICollection<SpecialListItem> Specials { get; set; } = new List<SpecialListItem>();
    }
}
