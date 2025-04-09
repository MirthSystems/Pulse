namespace Pulse.Core.Models.Responses
{
    public class SpecialDetailResponse
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public int TypeId { get; set; }
        public string StartDate { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string? EndTime { get; set; }
        public string? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public string? TimeRemaining { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurringSchedule { get; set; }
        public List<TagDetail> Tags { get; set; } = new List<TagDetail>();
    }
}
