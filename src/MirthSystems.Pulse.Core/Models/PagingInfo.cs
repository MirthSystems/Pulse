namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents pagination metadata for API responses.
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// Gets or sets the current page number (1-based).
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page available.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page available.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Creates a new paging info object with calculated total pages.
        /// </summary>
        /// <param name="currentPage">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total count of items across all pages.</param>
        /// <returns>A configured paging info object with all properties set.</returns>
        public static PagingInfo Create(int currentPage, int pageSize, int totalCount)
        {
            return new PagingInfo
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)Math.Max(1, pageSize))
            };
        }
    }
}
