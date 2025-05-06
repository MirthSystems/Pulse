namespace MirthSystems.Pulse.Core.Models.Responses
{
    /// <summary>
    /// Generic API response wrapper for paginated data collections.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated collection.</typeparam>
    /// <remarks>
    /// <para>This class standardizes paginated API responses across the application with the following features:</para>
    /// <para>- Success indicator to quickly determine if the request succeeded</para>
    /// <para>- Optional message providing details about the response</para>
    /// <para>- Collection of strongly-typed data items</para>
    /// <para>- Pagination metadata for navigating through the collection</para>
    /// <para>- Factory methods for creating success and error responses consistently</para>
    /// <para>Usage examples:</para>
    /// <para>- Successful venue listing: PagedApiResponse&lt;VenueListItem&gt;.CreateSuccess(venues, 1, 20, 57, "Venues retrieved successfully")</para>
    /// <para>- Failed search: PagedApiResponse&lt;SpecialListItem&gt;.CreateError("Unable to geocode the provided address")</para>
    /// </remarks>
    public class PagedApiResponse<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        /// <remarks>
        /// <para>When true, indicates that the operation completed successfully and the Data property contains results.</para>
        /// <para>When false, indicates that an error occurred and the Message property contains error information.</para>
        /// <para>Example: true for a successful venue search, false when a search location couldn't be geocoded.</para>
        /// </remarks>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Gets or sets an optional message providing additional information about the response.
        /// </summary>
        /// <remarks>
        /// <para>For successful responses, this may contain confirmation or additional context.</para>
        /// <para>For error responses, this contains the error message explaining what went wrong.</para>
        /// <para>Examples:</para>
        /// <para>- Success: "Venues retrieved successfully"</para>
        /// <para>- Error: "Could not geocode address: 123 Invalid Street"</para>
        /// </remarks>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the collection of items returned by the API.
        /// </summary>
        /// <remarks>
        /// <para>For successful responses, this contains the requested data items of type T.</para>
        /// <para>For error responses, this is typically null or empty.</para>
        /// <para>This collection only contains items for the current page, not all available items.</para>
        /// <para>Examples for T:</para>
        /// <para>- VenueListItem: List of venues for the current page</para>
        /// <para>- SpecialListItem: List of special promotions for the current page</para>
        /// </remarks>
        public ICollection<T>? Data { get; set; }

        /// <summary>
        /// Gets or sets the pagination metadata for navigating through the collection.
        /// </summary>
        /// <remarks>
        /// <para>This property provides information about the current page, total pages, and navigation capabilities.</para>
        /// <para>It includes:</para>
        /// <para>- Current page number</para>
        /// <para>- Page size (items per page)</para>
        /// <para>- Total count of all items (across all pages)</para>
        /// <para>- Total number of pages</para>
        /// <para>- Indicators for whether previous/next pages exist</para>
        /// </remarks>
        public required PaginationData Pagination { get; set; }

        /// <summary>
        /// Creates a successful paged response with data and pagination information.
        /// </summary>
        /// <param name="data">The collection of data items for the current page.</param>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The page size (items per page).</param>
        /// <param name="totalCount">The total count of all items across all pages.</param>
        /// <param name="message">Optional success message providing additional context.</param>
        /// <returns>A successful paged API response containing the provided data and pagination information.</returns>
        /// <remarks>
        /// <para>Use this factory method to create standardized success responses with pagination.</para>
        /// <para>Examples:</para>
        /// <para>- PagedApiResponse&lt;VenueListItem&gt;.CreateSuccess(venues, 1, 20, 57, "Venues retrieved successfully")</para>
        /// <para>- PagedApiResponse&lt;SpecialListItem&gt;.CreateSuccess(specials, 2, 10, 25)</para>
        /// </remarks>
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
        /// Creates a failure response with an error message.
        /// </summary>
        /// <param name="message">The error message describing what went wrong.</param>
        /// <returns>A failure API response with the specified error message.</returns>
        /// <remarks>
        /// <para>Use this factory method to create standardized error responses.</para>
        /// <para>Examples:</para>
        /// <para>- PagedApiResponse&lt;VenueListItem&gt;.CreateError("Invalid search parameters")</para>
        /// <para>- PagedApiResponse&lt;SpecialListItem&gt;.CreateError("Could not geocode address: 123 Invalid Street")</para>
        /// </remarks>
        public static PagedApiResponse<T> CreateError(string message) =>
            new PagedApiResponse<T>
            {
                Success = false,
                Message = message,
                Pagination = PaginationData.Create(0, 0, 0)
            };
    }
}
