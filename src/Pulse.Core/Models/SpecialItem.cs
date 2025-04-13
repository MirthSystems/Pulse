namespace Pulse.Core.Models
{
    using NodaTime;
    using Pulse.Core.Enums;
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Response model for special information
    /// </summary>
    public class SpecialItem
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public SpecialTypes Type { get; set; }
        public LocalDate StartDate { get; set; }
        public LocalTime StartTime { get; set; }
        public LocalTime? EndTime { get; set; }
        public LocalDate? ExpirationDate { get; set; }
        public bool IsRecurring { get; set; }
        public Period? RecurringPeriod { get; set; }
        public int? ActiveDaysOfWeek { get; set; }
        public long VenueId { get; set; }
    }
}
