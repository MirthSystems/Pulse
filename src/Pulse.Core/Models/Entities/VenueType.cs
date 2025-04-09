namespace Pulse.Core.Models.Entities
{
    /// <summary>
    /// Represents a classification for venues such as bar, restaurant, cafe, etc.
    /// </summary>
    public class VenueType
    {
        public int Id { get; set; }

        /// <summary>
        /// The name of this venue type
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Bar"</para>
        /// <para>- "Restaurant"</para>
        /// <para>- "Cafe"</para>
        /// <para>- "Club"</para>
        /// <para>- "Brewery"</para>
        /// </remarks>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Optional description of this venue type
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// List of venues of this type
        /// </summary>
        public virtual List<Venue> Venues { get; set; } = [];
    }
}
