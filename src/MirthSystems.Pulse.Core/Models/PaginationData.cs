namespace MirthSystems.Pulse.Core.Models
{
    public class PaginationData
    {
        /// <summary>
        /// The current page number
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public int Page { get; set; }

        /// <summary>
        /// The number of items per page
        /// </summary>
        /// <remarks>e.g. 20</remarks>
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of items across all pages
        /// </summary>
        /// <remarks>e.g. 57</remarks>
        public int TotalCount { get; set; }

        /// <summary>
        /// The total number of pages
        /// </summary>
        /// <remarks>e.g. 3</remarks>
        public int TotalPages { get; set; }

        /// <summary>
        /// Whether there is a previous page available
        /// </summary>
        /// <remarks>e.g. false</remarks>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Whether there is a next page available
        /// </summary>
        /// <remarks>e.g. true</remarks>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Creates a new pagination data object
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="totalCount">The total count of items</param>
        /// <returns>A pagination data object</returns>
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
