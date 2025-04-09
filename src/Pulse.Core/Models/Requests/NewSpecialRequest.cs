namespace Pulse.Core.Models.Requests
{
    using NodaTime;
    using Pulse.Core.Enums;
    using System.ComponentModel.DataAnnotations;

    public class NewSpecialRequest
    {
        [Required]
        public int VenueId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Content { get; set; }

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

        [StringLength(50)]
        public string? RecurringSchedule { get; set; }

        public List<TagItem> Tags { get; set; } = new List<TagItem>();
    }
}
