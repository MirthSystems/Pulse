namespace MirthSystems.Pulse.Core.Models.Responses
{
    /// <summary>
    /// Generic API response wrapper that provides a consistent structure for all API responses.
    /// </summary>
    /// <typeparam name="T">The type of data being returned in the response.</typeparam>
    /// <remarks>
    /// <para>This class standardizes API responses across the application with the following features:</para>
    /// <para>- Success indicator to quickly determine if the request succeeded</para>
    /// <para>- Optional message providing details about the response</para>
    /// <para>- Strongly-typed data payload when applicable</para>
    /// <para>- Factory methods for creating success and error responses consistently</para>
    /// <para>Usage examples:</para>
    /// <para>- Successful venue retrieval: ApiResponse&lt;VenueDetail&gt;.CreateSuccess(venueDetail, "Venue retrieved successfully")</para>
    /// <para>- Failed operation: ApiResponse&lt;object&gt;.CreateError("Venue not found")</para>
    /// </remarks>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        /// <remarks>
        /// <para>When true, indicates that the operation completed successfully and the Data property may contain results.</para>
        /// <para>When false, indicates that an error occurred and the Message property contains error information.</para>
        /// <para>Example: true for a successful venue lookup, false when a requested venue doesn't exist.</para>
        /// </remarks>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Gets or sets an optional message providing additional information about the response.
        /// </summary>
        /// <remarks>
        /// <para>For successful responses, this may contain confirmation or additional context.</para>
        /// <para>For error responses, this contains the error message explaining what went wrong.</para>
        /// <para>Examples:</para>
        /// <para>- Success: "Venue created successfully"</para>
        /// <para>- Error: "Venue with ID 12345 not found"</para>
        /// </remarks>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the data returned by the API operation.
        /// </summary>
        /// <remarks>
        /// <para>For successful responses, this contains the requested data of type T.</para>
        /// <para>For error responses, this is typically null.</para>
        /// <para>Examples for T:</para>
        /// <para>- VenueDetail: Complete information about a venue</para>
        /// <para>- bool: Result of a delete operation</para>
        /// <para>- SpecialDetail: Information about a special promotion</para>
        /// </remarks>
        public T? Data { get; set; }

        /// <summary>
        /// Creates a successful response with data and an optional message.
        /// </summary>
        /// <param name="data">The data to return in the response.</param>
        /// <param name="message">Optional success message providing additional context.</param>
        /// <returns>A successful API response containing the provided data.</returns>
        /// <remarks>
        /// <para>Use this factory method to create standardized success responses.</para>
        /// <para>Examples:</para>
        /// <para>- ApiResponse&lt;VenueDetail&gt;.CreateSuccess(venue, "Venue retrieved successfully")</para>
        /// <para>- ApiResponse&lt;bool&gt;.CreateSuccess(true, "Venue deleted successfully")</para>
        /// </remarks>
        public static ApiResponse<T> CreateSuccess(T data, string? message = null) =>
            new ApiResponse<T> { 
                Success = true, 
                Data = data, 
                Message = message 
            };

        /// <summary>
        /// Creates a failure response with an error message.
        /// </summary>
        /// <param name="message">The error message describing what went wrong.</param>
        /// <returns>A failure API response with the specified error message.</returns>
        /// <remarks>
        /// <para>Use this factory method to create standardized error responses.</para>
        /// <para>Examples:</para>
        /// <para>- ApiResponse&lt;object&gt;.CreateError("Invalid venue ID format")</para>
        /// <para>- ApiResponse&lt;VenueDetail&gt;.CreateError("Venue with ID 12345 not found")</para>
        /// </remarks>
        public static ApiResponse<T> CreateError(string message) =>
            new ApiResponse<T> { 
                Success = false, 
                Message = message 
            };
    }
}
