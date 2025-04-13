namespace Pulse.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NodaTime;
    using Pulse.Core.Enums;

    /// <summary>
    /// Request model for creating a new special
    /// </summary>
    public class NewSpecialRequest
    {
        [Required]
        [StringLength(255)]
        public string Content { get; set; } = null!;

        [Required]
        public SpecialTypes Type { get; set; }

        [Required]
        public LocalDate StartDate { get; set; }

        [Required]
        public LocalTime StartTime { get; set; }

        public LocalTime? EndTime { get; set; }

        public LocalDate? ExpirationDate { get; set; }

        [Required]
        public bool IsRecurring { get; set; }

        public Period? RecurringPeriod { get; set; }

        public int? ActiveDaysOfWeek { get; set; }

        [Required]
        public long VenueId { get; set; }

        public List<long>? TagIds { get; set; }
    }
}
