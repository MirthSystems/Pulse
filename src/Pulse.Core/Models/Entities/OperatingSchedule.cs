namespace Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents the operating schedule for a venue on a specific day of the week.
    /// </summary>
    public class OperatingSchedule
    {
        public int Id { get; set; }

        /// <summary>
        /// The venue this operating schedule entry belongs to.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// The day of the week this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>Uses the System.DayOfWeek enum with values:</para>
        /// <para>- Sunday (0)</para>
        /// <para>- Monday (1)</para>
        /// <para>- Tuesday (2)</para>
        /// <para>- Wednesday (3)</para>
        /// <para>- Thursday (4)</para>
        /// <para>- Friday (5)</para>
        /// <para>- Saturday (6)</para>
        /// </remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// The time the venue opens.
        /// </summary>
        /// <remarks>
        /// <para>Uses NodaTime.LocalTime to represent time of day without a date component.</para>
        /// <para>Examples:</para>
        /// <para>- new LocalTime(9, 0) // 9:00 AM</para>
        /// <para>- new LocalTime(17, 30) // 5:30 PM</para>
        /// <para>- new LocalTime(0, 0) // 12:00 AM (midnight)</para>
        /// <para>- LocalTime.FromHourMinuteSecondMillisecond(14, 30, 0, 0) // 2:30 PM</para>
        /// </remarks>
        public LocalTime TimeOfOpen { get; set; }

        /// <summary>
        /// The time the venue closes.
        /// </summary>
        /// <remarks>
        /// <para>Uses NodaTime.LocalTime to represent time of day without a date component.</para>
        /// <para>Examples:</para>
        /// <para>- new LocalTime(23, 0) // 11:00 PM</para>
        /// <para>- new LocalTime(2, 0) // 2:00 AM (for venues closing after midnight)</para>
        /// <para>- new LocalTime(23, 59) // 11:59 PM (for 24-hour venues, use 23:59)</para>
        /// <para>Note: For venues closing after midnight, use the closing time on the same day.</para>
        /// <para>For example, a venue open from 8 PM to 2 AM should have:</para>
        /// <para>- One record for the day with TimeOfOpen = 20:00, TimeOfClose = 23:59</para>
        /// <para>- One record for the next day with TimeOfOpen = 00:00, TimeOfClose = 02:00</para>
        /// </remarks>
        public LocalTime TimeOfClose { get; set; }

        /// <summary>
        /// Indicates if the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When set to true, the TimeOfOpen and TimeOfClose values are ignored.</para>
        /// <para>Examples:</para>
        /// <para>- true // Venue is closed on this day</para>
        /// <para>- false // Venue is open on this day</para>
        /// </remarks>
        public bool IsClosed { get; set; }

        /// <summary>
        /// The venue this operating schedule entry is associated with.
        /// </summary>
        public virtual Venue Venue { get; set; } = null!;
    }
}
