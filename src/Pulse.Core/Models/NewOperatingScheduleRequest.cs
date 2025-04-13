namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using NodaTime;

    /// <summary>
    /// Request model for creating a new operating schedule
    /// </summary>
    public class NewOperatingScheduleRequest
    {
        [Required]
        public required DayOfWeek DayOfWeek { get; set; }

        [Required]
        public required LocalTime TimeOfOpen { get; set; }

        [Required]
        public required LocalTime TimeOfClose { get; set; }

        [Required]
        public required bool IsClosed { get; set; }
    }
}
