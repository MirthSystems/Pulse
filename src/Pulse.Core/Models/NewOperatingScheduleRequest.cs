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
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public LocalTime TimeOfOpen { get; set; }

        [Required]
        public LocalTime TimeOfClose { get; set; }

        [Required]
        public bool IsClosed { get; set; }
    }
}
