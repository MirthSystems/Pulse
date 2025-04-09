namespace Pulse.Core.Models.Responses
{
    public class VenueDetailResponse
    {
        public int Id { get; set; }
        public int VenueTypeId { get; set; }
        public string VenueTypeName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? ImageLink { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string Locality { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string FormattedAddress { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<OperatingHoursResponse> OperatingHours { get; set; } = new List<OperatingHoursResponse>();
        public List<SpecialSummary> ActiveSpecials { get; set; } = new List<SpecialSummary>();
        public bool UserCanManage { get; set; }
        public bool UserCanManageSpecials { get; set; }
    }
}
