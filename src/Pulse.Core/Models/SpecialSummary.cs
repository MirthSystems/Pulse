namespace Pulse.Core.Models
{
    public class SpecialSummary
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string? EndTime { get; set; }
        public bool IsActive { get; set; }
        public string? TimeRemaining { get; set; }
        public List<string> TagNames { get; set; } = new List<string>();
        public bool IsRecurring { get; set; }
    }
}
