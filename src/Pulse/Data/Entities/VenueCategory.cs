namespace Pulse.Data.Entities
{
    public class VenueCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public IList<Venue> Venues { get; set; } = new List<Venue>();
    }
}
