namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOperatingScheduleRequest
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        [Required]
        [Range(0, 6)]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfOpen { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfClose { get; set; } = string.Empty;

        [Required]
        public bool IsClosed { get; set; }
    }
}
