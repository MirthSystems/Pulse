namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing operating schedule entry.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the fields that can be modified when updating an operating schedule entry.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Unlike CreateOperatingScheduleRequest, this does not include VenueId or DayOfWeek as those are immutable.</para>
    /// </remarks>
    public class UpdateOperatingScheduleRequest
    {
        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "09:00" (9 AM), "17:30" (5:30 PM)</para>
        /// <para>This property is required even when IsClosed is true, though it will be ignored in that case.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfOpen { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "02:00" (2 AM the next day)</para>
        /// <para>This property is required even when IsClosed is true, though it will be ignored in that case.</para>
        /// <para>If this time is earlier than TimeOfOpen, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfClose { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the TimeOfOpen and TimeOfClose properties.</para>
        /// </remarks>
        [Required]
        public bool IsClosed { get; set; }
    }
}
