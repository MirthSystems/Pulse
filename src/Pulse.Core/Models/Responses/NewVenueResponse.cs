namespace Pulse.Core.Models.Responses
{
    public class NewVenueResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int VenueTypeId { get; set; }

        public string VenueTypeName { get; set; } = null!;

        public string FormattedAddress { get; set; } = null!;

        public bool UserCanManage { get; set; }
    }
}
