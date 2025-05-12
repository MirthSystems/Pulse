namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents a collection of specials for a venue.
    /// </summary>
    public class SpecialMenu
    {
        /// <summary>
        /// Gets or sets the list of specials offered by a venue.
        /// </summary>
        public List<SpecialItem> Items { get; set; } = new List<SpecialItem>();
    }
}
