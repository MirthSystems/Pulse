namespace MirthSystems.Pulse.Core.DataTransferObjects
{
    using System;
    using NodaTime;

    public class OperatingScheduleDataTransferObject
    {
        /// <summary>
        /// The unique identifier for the operating schedule
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The venue this schedule belongs to
        /// </summary>
        /// <remarks>e.g. 5</remarks>
        public long VenueId { get; set; }

        /// <summary>
        /// The day of the week this schedule applies to
        /// </summary>
        /// <remarks>e.g. Monday</remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// The time when the venue opens on this day (in 24-hour format)
        /// </summary>
        /// <remarks>e.g. 09:00</remarks>
        public LocalTime TimeOfOpen { get; set; }

        /// <summary>
        /// The time when the venue closes on this day (in 24-hour format)
        /// </summary>
        /// <remarks>e.g. 22:00</remarks>
        public LocalTime TimeOfClose { get; set; }

        /// <summary>
        /// Whether the venue is closed on this day
        /// </summary>
        /// <remarks>e.g. false</remarks>
        public bool IsClosed { get; set; }
    }
}
