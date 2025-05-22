namespace Pulse.Data.Entities
{
    using NetTopologySuite.Geometries;

    using NodaTime;

    public class Venue
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }
        public required string StreetAddress { get; set; }
        public string? SecondaryAddress { get; set; }
        public required string Locality { get; set; }
        public required string Region { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
        public Point? Location { get; set; }
        public Instant CreatedAt { get; set; }
        public required string CreatedByUserId { get; set; }
        public Instant? UpdatedAt { get; set; }
        public string? UpdatedByUserId { get; set; }
        public bool IsActive { get; set; } = true;
        public IList<BusinessHours> BusinessHours { get; set; } = new List<BusinessHours>();
        public VenueCategory? Category { get; set; }
    }
}
