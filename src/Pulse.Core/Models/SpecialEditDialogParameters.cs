namespace Pulse.Core.Models
{
    public class SpecialEditDialogParameters
    {
        public bool IsNew { get; set; }
        public long VenueId { get; set; }
        public SpecialWithDetails? Special { get; set; }
    }
}
