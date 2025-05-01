namespace MirthSystems.Pulse.Core.Models.Entities
{
    using NodaTime;

    /// <summary>
    /// Represents business hours for a venue on a specific day of the week.
    /// </summary>
    /// <remarks>
    /// <para>Each venue will have up to 7 OperatingSchedule entries, one for each day of the week.</para>
    /// <para>This entity stores regular business hours and closed status for each day.</para>
    /// <para>Future extensions may include holiday or special event overrides in a separate table.</para>
    /// </remarks>
    public class OperatingSchedule
    {
        /// <summary>
        /// Gets or sets the unique identifier for the operating schedule entry.
        /// </summary>
        /// <remarks>
        /// This is the primary key for the OperatingSchedule entity in the database.
        /// </remarks>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the Venue entity.
        /// </summary>
        /// <remarks>
        /// <para>This represents the venue to which this operating schedule applies.</para>
        /// <para>This is a required field and is used to establish the relationship with the Venue entity.</para>
        /// </remarks>
        public long VenueId { get; set; }

        /// <summary>
        /// Gets or sets the day of the week for this operating schedule entry.
        /// </summary>
        /// <remarks>
        /// <para>This uses the standard .NET System.DayOfWeek enum.</para>
        /// <para>The values are: Sunday (0), Monday (1), Tuesday (2), etc.</para>
        /// </remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>This represents the time when the venue opens on the specified day.</para>
        /// <para>Uses NodaTime's LocalTime to accurately represent time-of-day without date or timezone.</para>
        /// <para>This property is not relevant when IsClosed is true.</para>
        /// </remarks>
        public LocalTime TimeOfOpen { get; set; }

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>This represents the time when the venue closes on the specified day.</para>
        /// <para>Uses NodaTime's LocalTime to accurately represent time-of-day without date or timezone.</para>
        /// <para>If this is earlier than TimeOfOpen, it's interpreted as closing after midnight (the next day).</para>
        /// <para>This property is not relevant when IsClosed is true.</para>
        /// </remarks>
        public LocalTime TimeOfClose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the TimeOfOpen and TimeOfClose properties.</para>
        /// </remarks>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated Venue.
        /// </summary>
        /// <remarks>
        /// <para>This navigation property links to the venue to which these business hours apply.</para>
        /// <para>It provides access to the venue's details such as name, address, and other properties.</para>
        /// </remarks>
        public required virtual Venue Venue { get; set; }
    }
}
