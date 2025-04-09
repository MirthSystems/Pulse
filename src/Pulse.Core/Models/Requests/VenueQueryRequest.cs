namespace Pulse.Core.Models.Requests
{
    public class VenueQueryRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? VenueTypeId { get; set; }
    }
}
