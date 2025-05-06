namespace MirthSystems.Pulse.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the complete set of operating hours for a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model aggregates all operating schedules for a venue to provide a complete view of when the venue is open.</para>
    /// <para>It includes the venue's identity information and a collection of individual schedule items for each day of the week.</para>
    /// <para>Used primarily for display purposes in the user interface and API responses.</para>
    /// </remarks>
    public class BusinessHours
    {
        /// <summary>
        /// Gets or sets the unique identifier of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue.
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

        /// <summary>
        /// Gets or sets the collection of operating schedule items for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection typically contains seven items, one for each day of the week.</para>
        /// <para>Each item describes the opening and closing times for a specific day or indicates if the venue is closed on that day.</para>
        /// <para>The collection is ordered by day of week, starting from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        [Required]
        public ICollection<OperatingScheduleListItem> ScheduleItems { get; set; } = new List<OperatingScheduleListItem>();
    }
}
