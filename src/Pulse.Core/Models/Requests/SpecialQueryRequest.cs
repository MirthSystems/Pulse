namespace Pulse.Core.Models.Requests
{
    public class SpecialQueryRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? VenueId { get; set; }
        public int? SpecialTypeId { get; set; }
        public int? TagId { get; set; }
    }
}
