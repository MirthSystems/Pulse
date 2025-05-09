namespace MirthSystems.Pulse.Core.Models.Requests
{
    using MirthSystems.Pulse.Core.Enums;

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new special promotion.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the required and optional fields for creating a new special entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>It supports both one-time specials and recurring specials with CRON-based scheduling.</para>
    /// </remarks>
    public class CreateSpecialRequest
    {
        /// <summary>
        /// Gets or sets the ID of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>This field associates the special with a specific venue.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

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
        [StringLength(200, MinimumLength = 5)]
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
        [Required]
        [EnumDataType(typeof(SpecialTypes))]
        public SpecialTypes Type { get; set; }

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
        /// <para>If this time is earlier than StartTime, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string? EndTime { get; set; }

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
        public string? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the special recurs over time.
        /// </summary>
        /// <remarks>
        /// <para>True if the special repeats on a schedule (e.g., weekly happy hour).</para>
        /// <para>False if the special is a one-time event (e.g., New Year's Eve party).</para>
        /// <para>When true, the CronSchedule property should be provided.</para>
        /// </remarks>
        [Required]
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
        [RegularExpression(@"^(\*|[0-9,-/]+)(\s+(\*|[0-9,-/]+)){4,5}$", ErrorMessage = "Invalid CRON expression.")]
        [StringLength(100)]
        public string? CronSchedule { get; set; }
    }
}
