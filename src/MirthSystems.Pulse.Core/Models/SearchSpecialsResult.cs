namespace MirthSystems.Pulse.Core.Models
{
    public class SearchSpecialsResult
    {
        public VenueItemExtended Venue { get; set; } = new VenueItemExtended();

        public SpecialMenu Specials { get; set; } = new SpecialMenu();
    }
}
