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
        public required string Content { get; set; }

        [Required]
        public required SpecialTypes Type { get; set; }

        [Required]
        public required LocalDate StartDate { get; set; }

        [Required]
        public required LocalTime StartTime { get; set; }

        public LocalTime? EndTime { get; set; }

        public LocalDate? ExpirationDate { get; set; }

        [Required]
        public required bool IsRecurring { get; set; }

        public Period? RecurringPeriod { get; set; }

        public int? ActiveDaysOfWeek { get; set; }

        [Required]
        public required long VenueId { get; set; }

        public List<long>? TagIds { get; set; }
    }
}
