namespace Pulse.Data.Entities
{
    using System;

    using NodaTime;

    public class BusinessHours
    {
        public long Id { get; set; }
        public long VenueId { get; set; }
        public int DayOfWeekId { get; set; }
        public LocalTime? OpenTime { get; set; }
        public LocalTime? CloseTime { get; set; }
        public bool IsClosed { get; set; }
        public Venue? Venue { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
    }
}
