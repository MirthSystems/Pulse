namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using MirthSystems.Pulse.Core.Enums;

    public class SpecialDetail
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        public SpecialTypes Type { get; set; }

        [Required]
        public string TypeName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string ExpirationDate { get; set; } = string.Empty;

        public bool IsRecurring { get; set; }

        [StringLength(100)]
        public string CronSchedule { get; set; } = string.Empty;

        public bool IsCurrentlyRunning { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
