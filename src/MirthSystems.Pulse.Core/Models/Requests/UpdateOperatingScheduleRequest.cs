namespace MirthSystems.Pulse.Core.Models.Requests
// No changes needed; all properties already have appropriate data annotations.
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing operating schedule
    /// </summary>
    public class UpdateOperatingScheduleRequest
    {
        /// <summary>
        /// The time when the venue opens on this day (in HH:mm format)
        /// </summary>
        /// <remarks>e.g. 09:00</remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public required string TimeOfOpen { get; set; }

        /// <summary>
        /// The time when the venue closes on this day (in HH:mm format)
        /// </summary>
        /// <remarks>e.g. 22:00</remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public required string TimeOfClose { get; set; }

        /// <summary>
        /// Whether the venue is closed on this day
        /// </summary>
        /// <remarks>e.g. false</remarks>
        [Required]
        public bool IsClosed { get; set; }
    }
}
