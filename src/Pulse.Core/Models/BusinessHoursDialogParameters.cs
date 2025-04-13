namespace Pulse.Core.Models
{
    public class BusinessHoursDialogParameters
    {
        public long VenueId { get; set; }
        public List<OperatingScheduleItem>? BusinessHours { get; set; }
    }
}
