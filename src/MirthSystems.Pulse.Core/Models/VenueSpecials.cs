namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    
    /// <summary>
    /// Represents all special promotions associated with a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model aggregates all special promotions for a venue to provide a complete view of what the venue is offering.</para>
    /// <para>It includes the venue's identity information and a collection of special promotions.</para>
    /// <para>Used primarily for display purposes in the user interface and API responses.</para>
    /// </remarks>
    public class VenueSpecials
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
        /// Gets or sets the collection of special promotions for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection contains all current and upcoming specials for the venue.</para>
        /// <para>Each item describes a specific promotional offer or event, including its timing and status.</para>
        /// <para>The collection may include specials that are currently active, scheduled for the future, or recurring.</para>
        /// </remarks>
        [Required]
        public ICollection<SpecialListItem> Specials { get; set; } = new List<SpecialListItem>();
    }
}
