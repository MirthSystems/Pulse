namespace MirthSystems.Pulse.Core.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Response model for venue business hours
    /// </summary>
    public class BusinessHours
    {
        /// <summary>
        /// The venue ID these business hours belong to
        /// </summary>
        /// <remarks>e.g. 5</remarks>
        public long VenueId { get; set; }

        /// <summary>
        /// The name of the venue
        /// </summary>
        /// <remarks>e.g. The Rusty Anchor Pub</remarks>
        public required string VenueName { get; set; }

        /// <summary>
        /// The operating schedules for each day of the week
        /// </summary>
        public required ICollection<OperatingScheduleListItem> ScheduleItems { get; set; }
    }
}
