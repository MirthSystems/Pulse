namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents a paginated result set for API responses.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the collection of items for the current page.
        /// </summary>
        public ICollection<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the pagination information.
        /// </summary>
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}