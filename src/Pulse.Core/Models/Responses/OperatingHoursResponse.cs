namespace Pulse.Core.Models.Responses
{
    using NodaTime;

    public class OperatingHoursResponse
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public LocalTime TimeOfOpen { get; set; }
        public LocalTime TimeOfClose { get; set; }
        public bool IsClosed { get; set; }
        public string FormattedHours { get; set; } = null!;
    }
}
