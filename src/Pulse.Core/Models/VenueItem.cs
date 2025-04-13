namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Response model for venue information
    /// </summary>
    public class VenueItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string Locality { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? ImageLink { get; set; }
        public int VenueTypeId { get; set; }
        public string? VenueTypeName { get; set; }
    }
}
