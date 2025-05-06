namespace MirthSystems.Pulse.Core.Entities
{
    using MirthSystems.Pulse.Core.Enums;

    using NodaTime;

    /// <summary>
    /// Represents a special event, promotion, or offer at a venue, such as happy hours, live performances, or limited-time discounts.
    /// This entity supports both one-time and recurring specials, with flexible options for defining start times, end times, and expiration dates.
    /// All date and time properties are stored as local values (without timezone), with the expectation that the frontend will handle timezone calculations
    /// based on the venue's location and the user's location.
    /// </summary>
    public class Special
    {
        public long Id { get; set; }

        public long VenueId { get; set; }

        /// <summary>
        /// A brief description of the special.
        /// This required field provides details about what the special entails, making it clear to users what is being offered.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Half-Price Wings Happy Hour"</para>
        /// <para>- "Live Jazz Night"</para>
        /// <para>- "Buy One Get One Free Draft Beer"</para>
        /// </remarks>
        public required string Content { get; set; }

        /// <summary>
        /// The category of the special, such as Food, Drink, or Entertainment.
        /// This required field helps classify and filter specials for users.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- SpecialTypes.Food</para>
        /// <para>- SpecialTypes.Drink</para>
        /// <para>- SpecialTypes.Entertainment</para>
        /// </remarks>
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// The start date of the special.
        /// For one-time specials, this is the event date. For recurring specials, this is the first occurrence.
        /// The date is interpreted in the venue's timezone, which is derived from the venue's location on the frontend.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- Concert: LocalDate(2023, 12, 15)</para>
        /// <para>- Weekly happy hour: LocalDate(2023, 11, 7)</para>
        /// </remarks>
        public LocalDate StartDate { get; set; }

        /// <summary>
        /// The start time of the special on the start date or each recurrence.
        /// This required field is interpreted in the venue's timezone, derived from the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 8 PM concert: LocalTime(20, 0)</para>
        /// <para>- 5 PM happy hour: LocalTime(17, 0)</para>
        /// </remarks>
        public LocalTime StartTime { get; set; }

        /// <summary>
        /// The end time of the special on the same day, or null if it spans multiple days or has no daily end.
        /// This optional field is interpreted in the venue's timezone, derived from the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 10 PM concert end: LocalTime(22, 0)</para>
        /// <para>- 7 PM happy hour end: LocalTime(19, 0)</para>
        /// <para>- Multi-day special: null</para>
        /// </remarks>
        public LocalTime? EndTime { get; set; }

        /// <summary>
        /// The expiration date of the special, or null if ongoing or same-day.
        /// For one-time specials, this is the end date if multi-day. For recurring specials, this is the last occurrence.
        /// This optional field is interpreted in the venue's timezone, derived from the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- Multi-day sale: LocalDate(2023, 11, 3)</para>
        /// <para>- Recurring end: LocalDate(2024, 3, 1)</para>
        /// <para>- Same-day special: null</para>
        /// </remarks>
        public LocalDate? ExpirationDate { get; set; }

        /// <summary>
        /// Determines whether the special repeats over time.
        /// If false, the special is a one-time event. If true, it recurs according to the CronSchedule.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- One-time concert: false</para>
        /// <para>- Weekly happy hour: true</para>
        /// </remarks>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// A CRON expression defining the recurrence schedule for the special, or null if not recurring.
        /// This field provides powerful and flexible scheduling capabilities using standard CRON syntax.
        /// </summary>
        /// <remarks>
        /// <para>Common examples for venue specials:</para>
        /// <para>- Daily at 5 PM: "0 17 * * *"</para>
        /// <para>- Every Monday at 8 PM: "0 20 * * 1"</para>
        /// <para>- Weekdays at 4 PM: "0 16 * * 1-5"</para>
        /// <para>- First Saturday of the month at 9 PM: "0 21 1-7 * 6"</para>
        /// <para>- Every 15th of the month at noon: "0 12 15 * *"</para>
        /// </remarks>
        public string? CronSchedule { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the special was created.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-04-01T09:00:00Z" for a special created on April 1, 2023.</para>
        /// </remarks>
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the special.
        /// </summary>
        /// <remarks>
        /// <para>Example: "auth0|12345" for the user who created the special.</para>
        /// </remarks>
        public required string CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the special was last updated, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-04-15T14:00:00Z" for a special updated on April 15, 2023.</para>
        /// </remarks>
        public Instant? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who last updated the special, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "auth0|12345" for the user who last updated the special.</para>
        /// </remarks>
        public string? UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the special has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// <para>Default is false. When true, the special is considered deleted but remains in the database for auditing purposes.</para>
        /// </remarks>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the timestamp when the special was deleted, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-05-01T12:00:00Z" for a special deleted on May 1, 2023.</para>
        /// </remarks>
        public Instant? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who deleted the special, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "auth0|12345" for the user who performed the deletion.</para>
        /// </remarks>
        public string? DeletedByUserId { get; set; }

        /// <summary>
        /// The venue associated with the special.
        /// This navigation property provides access to the venue's details, such as its location for timezone derivation.
        /// </summary>
        public virtual Venue? Venue { get; set; }
    }
}
