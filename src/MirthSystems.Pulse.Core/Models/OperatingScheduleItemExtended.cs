namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents detailed information about a venue's operating schedule for a specific day.
    /// </summary>
    /// <remarks>
    /// <para>This model extends OperatingScheduleItem with additional venue information.</para>
    /// <para>It includes all fields from the base class plus venue reference information.</para>
    /// <para>Used for operating schedule management interfaces and detail views.</para>
    /// </remarks>
    public class OperatingScheduleItemExtended : OperatingScheduleItem
    {
        /// <summary>
        /// Gets or sets the ID of the venue this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue this schedule applies to.
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
    }
}
