namespace Pulse.Core.Models
{
    public class VenueEditDialogParameters
    {
        public bool IsNew { get; set; }
        public VenueWithDetails? VenueDetails { get; set; }
        public List<VenueTypeItem>? VenueTypes { get; set; }
    }
}
