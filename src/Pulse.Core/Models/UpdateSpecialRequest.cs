namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using NodaTime;
    using Pulse.Core.Enums;

    /// <summary>
    /// Request model for updating an existing special
    /// </summary>
    public class UpdateSpecialRequest
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

        public List<long>? TagIds { get; set; }
    }
}
