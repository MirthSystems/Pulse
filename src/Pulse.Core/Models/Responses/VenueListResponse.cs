namespace Pulse.Core.Models.Responses
{
    public class VenueListResponse
    {
        public List<VenueSummary> Venues { get; set; } = new List<VenueSummary>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
