namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents detailed information about a venue's operating schedule for a specific day of the week.
    /// </summary>
    /// <remarks>
    /// <para>This model provides comprehensive information about when a venue is open on a specific day.</para>
    /// <para>It includes venue identification, day of week, and operating hours information.</para>
    /// <para>Used for schedule management interfaces and API responses.</para>
    /// </remarks>
    public class OperatingScheduleDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier of the operating schedule.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the schedule's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the venue associated with this schedule.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue associated with this schedule.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the day of the week for this schedule entry.
        /// </summary>
        /// <remarks>
        /// <para>This uses the standard .NET System.DayOfWeek enum.</para>
        /// <para>The values are: Sunday (0), Monday (1), Tuesday (2), etc.</para>
        /// </remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the name of the day of the week.
        /// </summary>
        /// <remarks>
        /// <para>The string representation of the DayOfWeek enum value.</para>
        /// <para>Examples: "Sunday", "Monday", "Tuesday", etc.</para>
        /// </remarks>
        [Required]
        public string DayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "09:00" (9 AM), "17:30" (5:30 PM)</para>
        /// <para>This property is still populated even when IsClosed is true, but is not displayed in that case.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string OpenTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "02:00" (2 AM the next day)</para>
        /// <para>This property is still populated even when IsClosed is true, but is not displayed in that case.</para>
        /// <para>If this time is earlier than OpenTime, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string CloseTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the OpenTime and CloseTime properties.</para>
        /// </remarks>
        public bool IsClosed { get; set; }
    }
}
