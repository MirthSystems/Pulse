namespace MirthSystems.Pulse.Core.DataTransferObjects
{
    using System;
    using MirthSystems.Pulse.Core.Enums;

    public class SpecialDataTransferObject
    {
        /// <summary>
        /// The unique identifier for the special
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The venue this special belongs to
        /// </summary>
        /// <remarks>e.g. 5</remarks>
        public long VenueId { get; set; }

        /// <summary>
        /// The name of the venue this special belongs to
        /// </summary>
        /// <remarks>e.g. The Rusty Anchor Pub</remarks>
        public required string VenueName { get; set; }

        /// <summary>
        /// Brief description of the special
        /// </summary>
        /// <remarks>e.g. Half-Price Wings Happy Hour</remarks>
        public required string Content { get; set; }

        /// <summary>
        /// The category of the special
        /// </summary>
        /// <remarks>e.g. Drink</remarks>
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// The starting date of the special
        /// </summary>
        /// <remarks>e.g. 2025-05-01</remarks>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// The time when the special starts
        /// </summary>
        /// <remarks>e.g. 17:00</remarks>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// The time when the special ends, if applicable
        /// </summary>
        /// <remarks>e.g. 20:00</remarks>
        public TimeOnly? EndTime { get; set; }

        /// <summary>
        /// The final date when the special is valid, if applicable
        /// </summary>
        /// <remarks>e.g. 2025-08-31</remarks>
        public DateOnly? ExpirationDate { get; set; }

        /// <summary>
        /// Whether the special occurs regularly
        /// </summary>
        /// <remarks>e.g. true</remarks>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// The recurrence pattern in CRON format
        /// </summary>
        /// <remarks>e.g. 0 17 * * 1-5</remarks>
        public string? CronSchedule { get; set; }

        /// <summary>
        /// Whether the special is currently active
        /// </summary>
        /// <remarks>e.g. true</remarks>
        public bool IsCurrentlyRunning { get; set; }

        /// <summary>
        /// When the special was created
        /// </summary>
        /// <remarks>e.g. 2025-04-15T14:30:00Z</remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The user ID of who created the special
        /// </summary>
        /// <remarks>e.g. auth0|12345</remarks>
        public required string CreatedByUserId { get; set; }

        /// <summary>
        /// When the special was last updated, if applicable
        /// </summary>
        /// <remarks>e.g. 2025-04-20T10:15:00Z</remarks>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// The user ID of who last updated the special, if applicable
        /// </summary>
        /// <remarks>e.g. auth0|12345</remarks>
        public string? UpdatedByUserId { get; set; }
    }
}
