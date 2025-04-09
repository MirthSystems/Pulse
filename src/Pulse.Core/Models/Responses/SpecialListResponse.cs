namespace Pulse.Core.Models.Responses
{
    public class SpecialListResponse
    {
        public List<SpecialSummary> Specials { get; set; } = new List<SpecialSummary>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
