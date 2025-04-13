namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using NodaTime;

    /// <summary>
    /// Request model for updating an existing operating schedule
    /// </summary>
    public class UpdateOperatingScheduleRequest
    {
        [Required]
        public required long Id { get; set; }

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
