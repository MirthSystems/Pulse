namespace Pulse.Core.Models
{
    public class VenueSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VenueTypeName { get; set; } = null!;
        public string FormattedAddress { get; set; } = null!;
        public string? ImageLink { get; set; }
        public int ActiveSpecialsCount { get; set; }
        public int ActivePostsCount { get; set; }
        public bool UserCanManage { get; set; }
        public double? DistanceInMiles { get; set; }
    }
}
