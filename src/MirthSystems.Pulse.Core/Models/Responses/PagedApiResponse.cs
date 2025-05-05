namespace MirthSystems.Pulse.Core.Models.Responses
{
    /// <summary>
    /// API response wrapper for paginated data
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated list</typeparam>
    public class PagedApiResponse<T>
    {
        /// <summary>
        /// Whether the request was successful
        /// </summary>
        /// <remarks>e.g. true</remarks>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Optional message providing additional information about the response
        /// </summary>
        /// <remarks>e.g. "Venues retrieved successfully"</remarks>
        public string? Message { get; set; }

        /// <summary>
        /// The data returned by the API
        /// </summary>
        public ICollection<T>? Data { get; set; }

        /// <summary>
        /// Pagination information
        /// </summary>
        public required PaginationData Pagination { get; set; }

        /// <summary>
        /// Creates a successful paged response
        /// </summary>
        /// <param name="data">The data items</param>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="totalCount">The total count of items</param>
        /// <param name="message">Optional success message</param>
        /// <returns>A successful paged API response</returns>
        public static PagedApiResponse<T> CreateSuccess(
            ICollection<T> data,
            int page,
            int pageSize,
            int totalCount,
            string? message = null) =>
                new PagedApiResponse<T>
                {
                    Success = true,
                    Data = data,
                    Message = message,
                    Pagination = PaginationData.Create(page, pageSize, totalCount)
                };

        /// <summary>
        /// Creates a failure response
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>A failure API response</returns>
        public static PagedApiResponse<T> CreateError(string message) =>
            new PagedApiResponse<T>
            {
                Success = false,
                Message = message,
                Pagination = PaginationData.Create(0, 0, 0)
            };
    }
}
