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
        public long Id { get; set; }

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
