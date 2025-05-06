namespace MirthSystems.Pulse.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BusinessHours
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        [Required]
        public ICollection<OperatingScheduleListItem> ScheduleItems { get; set; } = new List<OperatingScheduleListItem>();
    }
}
