namespace MirthSystems.Pulse.Core.Models.Requests
{
    using MirthSystems.Pulse.Core.Enums;

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing special
    /// </summary>
    public class UpdateSpecialRequest
    {
        /// <summary>
        /// Brief description of the special
        /// </summary>
        /// <remarks>e.g. Half-Price Wings Happy Hour</remarks>
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public required string Content { get; set; }

        /// <summary>
        /// The category of the special
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        [Required]
        [EnumDataType(typeof(SpecialTypes))]
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// The starting date of the special (YYYY-MM-DD)
        /// </summary>
        /// <remarks>e.g. 2025-05-01</remarks>
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public required string StartDate { get; set; }

        /// <summary>
        /// The time when the special starts (HH:mm)
        /// </summary>
        /// <remarks>e.g. 17:00</remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public required string StartTime { get; set; }

        /// <summary>
        /// The time when the special ends, if applicable (HH:mm)
        /// </summary>
        /// <remarks>e.g. 20:00</remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string? EndTime { get; set; }

        /// <summary>
        /// The final date when the special is valid, if applicable (YYYY-MM-DD)
        /// </summary>
        /// <remarks>e.g. 2025-08-31</remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string? ExpirationDate { get; set; }

        /// <summary>
        /// Whether the special occurs regularly
        /// </summary>
        /// <remarks>e.g. true</remarks>
        [Required]
        public bool IsRecurring { get; set; }

        /// <summary>
        /// The recurrence pattern in CRON format
        /// </summary>
        /// <remarks>e.g. 0 17 * * 1-5</remarks>
        [StringLength(100)]
        public string? CronSchedule { get; set; }
    }
}
