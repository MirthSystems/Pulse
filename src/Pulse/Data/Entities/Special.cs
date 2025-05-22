namespace Pulse.Data.Entities
{
    using System.Collections.Generic;
    using NodaTime;

    public class Special
    {
        #region Identity and primary fields
        public long Id { get; set; }
        public long VenueId { get; set; }
        public int SpecialCategoryId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Timing information
        public LocalDate StartDate { get; set; }      // First occurrence date
        public LocalTime StartTime { get; set; }      // Daily start time
        public LocalTime? EndTime { get; set; }       // Daily end time (optional)
        public LocalDate? EndDate { get; set; }       // Last occurrence date (for recurring/limited time)
        #endregion

        #region Recurrence information
        public bool IsRecurring { get; set; }
        public string? CronSchedule { get; set; }     // "0 17 * * 1-5" for weekdays at 5 PM
        #endregion

        #region Audit information
        public Instant CreatedAt { get; set; }
        public required string CreatedByUserId { get; set; }
        public Instant? UpdatedAt { get; set; }
        public string? UpdatedByUserId { get; set; }
        public bool IsActive { get; set; } = true;
        public Instant? DeactivatedAt { get; set; }
        public string? DeactivatedByUserId { get; set; }
        #endregion

        #region Navigation properties
        public Venue? Venue { get; set; }
        public SpecialCategory? Category { get; set; }
        #endregion
    }
}
