namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new operating schedule entry for a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines when a venue is open on a specific day of the week.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Used both as a standalone API request and within venue creation requests.</para>
    /// </remarks>
    public class CreateOperatingScheduleRequest
    {
        /// <summary>
        /// Gets or sets the ID of the venue this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>This field is required for standalone operating schedule creation requests.</para>
        /// <para>When used within a CreateVenueRequest, this field is ignored as the venue ID is not yet available.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the day of the week this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>This uses the standard .NET System.DayOfWeek enum.</para>
        /// <para>The values are: Sunday (0), Monday (1), Tuesday (2), etc.</para>
        /// </remarks>
        [Required]
        [Range(0, 6)]
        public DayOfWeek DayOfWeek { get; set; }

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
