namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using MirthSystems.Pulse.Core.Enums;

    /// <summary>
    /// Represents detailed information about a special promotion.
    /// </summary>
    /// <remarks>
    /// <para>This model provides comprehensive information about a special for detailed display.</para>
    /// <para>It includes all fields needed for a special's detail page, including timing and recurrence information.</para>
    /// <para>Used for special detail pages and special management interfaces.</para>
    /// </remarks>
    public class SpecialDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the special's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue offering this special.
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
        /// Gets or sets the description of the special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Half-Price Wings Happy Hour"</para>
        /// <para>- "Live Jazz Night"</para>
        /// <para>- "Buy One Get One Free Draft Beer"</para>
        /// </remarks>
        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the special.
        /// </summary>
        /// <remarks>
        /// <para>Categorizes the special as one of:</para>
        /// <para>- Food: Food specials like discount meals or appetizers</para>
        /// <para>- Drink: Drink specials like happy hour or discount cocktails</para>
        /// <para>- Entertainment: Entertainment specials like live music or events</para>
        /// </remarks>
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the special type.
        /// </summary>
        /// <remarks>
        /// <para>The string representation of the Type enum value.</para>
        /// <para>Examples: "Food", "Drink", "Entertainment"</para>
        /// </remarks>
        [Required]
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-15", "2024-01-01"</para>
        /// <para>For one-time specials, this is the event date.</para>
        /// <para>For recurring specials, this is the first occurrence.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "20:30" (8:30 PM)</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "19:00" (7 PM), "23:00" (11 PM)</para>
        /// <para>May be empty for specials without a specific end time.</para>
        /// </remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-31", "2024-03-15"</para>
        /// <para>For one-time specials, this is typically the same as the start date.</para>
        /// <para>For recurring specials, this is the last date the special will be offered.</para>
        /// <para>May be empty for ongoing specials with no defined end date.</para>
        /// </remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string ExpirationDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special recurs over time.
        /// </summary>
        /// <remarks>
        /// <para>True if the special repeats on a schedule (e.g., weekly happy hour).</para>
        /// <para>False if the special is a one-time event (e.g., New Year's Eve party).</para>
        /// </remarks>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the CRON expression defining the recurrence pattern of the special.
        /// </summary>
        /// <remarks>
        /// <para>Only applicable when IsRecurring is true.</para>
        /// <para>Examples:</para>
        /// <para>- "0 17 * * 1-5" (weekdays at 5 PM)</para>
        /// <para>- "0 20 * * 3" (Wednesdays at 8 PM)</para>
        /// <para>- "0 16 * * 6,0" (weekends at 4 PM)</para>
        /// </remarks>
        [StringLength(100)]
        public string CronSchedule { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special is currently running.
        /// </summary>
        /// <remarks>
        /// <para>True if the special is active at the current time or the time of the query.</para>
        /// <para>False if the special is not currently active (upcoming or ended).</para>
        /// </remarks>
        public bool IsCurrentlyRunning { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the special was created.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-04-01T09:00:00Z" for a special created on April 1, 2023.</para>
        /// </remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the special was last updated, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-04-15T14:00:00Z" for a special updated on April 15, 2023.</para>
        /// </remarks>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
