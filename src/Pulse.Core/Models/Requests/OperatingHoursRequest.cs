namespace Pulse.Core.Models.Requests
{
    using NodaTime;
    using System.ComponentModel.DataAnnotations;

    public class OperatingHoursRequest
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        public LocalTime TimeOfOpen { get; set; }

        public LocalTime TimeOfClose { get; set; }

        public bool IsClosed { get; set; }
    }
}
