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
    public class SpecialItemExtended : SpecialItem
    {
        /// <summary>
        /// Gets or sets information about the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>This contains the venue's basic details like ID, name, and location.</para>
        /// <para>Used for displaying venue context alongside special preview.</para>
        /// </remarks>
        [Required]
        public VenueItem Venue { get; set; } = new VenueItem();

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
        public string? CronSchedule { get; set; }

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
