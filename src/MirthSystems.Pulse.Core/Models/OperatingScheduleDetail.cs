namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OperatingScheduleDetail
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public string DayName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string OpenTime { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string CloseTime { get; set; } = string.Empty;

        public bool IsClosed { get; set; }
    }
}
