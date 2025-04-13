namespace Pulse.Core.Models
{
    using NodaTime;
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Response model for operating schedule information
    /// </summary>
    public class OperatingScheduleItem
    {
        public long Id { get; set; }
        public long VenueId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public LocalTime TimeOfOpen { get; set; }
        public LocalTime TimeOfClose { get; set; }
        public bool IsClosed { get; set; }
    }
}
