namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents pagination metadata for paged API responses.
    /// </summary>
    /// <remarks>
    /// <para>This class provides information about the current page, total pages, and navigation capabilities.</para>
    /// <para>It is used to help clients build UI pagination controls and navigate through paged data.</para>
    /// <para>The class also includes derived properties to simplify page navigation logic.</para>
    /// </remarks>
    public class PaginationData
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        /// <remarks>
        /// <para>This is a 1-based index (first page is 1, not 0).</para>
        /// <para>Example: 1 means the first page of results.</para>
        /// </remarks>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <remarks>
        /// <para>This defines how many items are included in each page of results.</para>
        /// <para>Example: 20 means each page contains up to 20 items.</para>
        /// <para>The last page may contain fewer items if the total count is not evenly divisible by the page size.</para>
        /// </remarks>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        /// <remarks>
        /// <para>This represents the total count of all available items, not just those on the current page.</para>
        /// <para>Example: 57 means there are 57 total items available across all pages.</para>
        /// </remarks>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        /// <remarks>
        /// <para>This is calculated as ⌈TotalCount ÷ PageSize⌉ (ceiling division).</para>
        /// <para>Example: With 57 total items and a page size of 20, there would be 3 pages.</para>
        /// </remarks>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page available.
        /// </summary>
        /// <remarks>
        /// <para>True when the current page is not the first page (Page > 1).</para>
        /// <para>False when the current page is the first page (Page == 1).</para>
        /// <para>This simplifies UI rendering logic for pagination controls.</para>
        /// </remarks>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page available.
        /// </summary>
        /// <remarks>
        /// <para>True when the current page is not the last page (Page &lt; TotalPages).</para>
        /// <para>False when the current page is the last page (Page >= TotalPages).</para>
        /// <para>This simplifies UI rendering logic for pagination controls.</para>
        /// </remarks>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Creates a new pagination data object with calculated total pages.
        /// </summary>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total count of items across all pages.</param>
        /// <returns>A configured pagination data object with all properties set.</returns>
        /// <remarks>
        /// <para>This factory method simplifies the creation of pagination data with proper total pages calculation.</para>
        /// <para>It ensures the ceiling division operation is performed correctly when calculating TotalPages.</para>
        /// <para>Example usage: PaginationData.Create(2, 10, 25) creates pagination data for page 2 of 3 pages.</para>
        /// </remarks>
        public static PaginationData Create(int page, int pageSize, int totalCount)
        {
            return new PaginationData
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}
