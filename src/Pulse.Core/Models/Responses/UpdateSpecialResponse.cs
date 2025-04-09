namespace Pulse.Core.Models.Responses
{
    public class UpdateSpecialResponse
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Content { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public List<string> TagNames { get; set; } = new List<string>();
        public DateTime LastUpdated { get; set; }
    }
}
