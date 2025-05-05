namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Response model for an operating schedule in a list
    /// </summary>
    public class OperatingScheduleListItem
    {
        /// <summary>
        /// The unique identifier for the operating schedule
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The day of the week this schedule applies to
        /// </summary>
        /// <remarks>e.g. Monday</remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// The name of the day of week
        /// </summary>
        /// <remarks>e.g. Monday</remarks>
        public required string DayName { get; set; }

        /// <summary>
        /// The time when the venue opens on this day (in HH:mm format)
        /// </summary>
        /// <remarks>e.g. 09:00</remarks>
        public required string OpenTime { get; set; }

        /// <summary>
        /// The time when the venue closes on this day (in HH:mm format)
        /// </summary>
        /// <remarks>e.g. 22:00</remarks>
        public required string CloseTime { get; set; }

        /// <summary>
        /// Whether the venue is closed on this day
        /// </summary>
        /// <remarks>e.g. false</remarks>
        public bool IsClosed { get; set; }
    }
}
