namespace MirthSystems.Pulse.Core.Models.Responses
{
    /// <summary>
    /// Generic API response wrapper
    /// </summary>
    /// <typeparam name="T">The type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Whether the request was successful
        /// </summary>
        /// <remarks>e.g. true</remarks>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Optional message providing additional information about the response
        /// </summary>
        /// <remarks>e.g. "Venue created successfully"</remarks>
        public string? Message { get; set; }

        /// <summary>
        /// The data returned by the API
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        /// <param name="data">The data to return</param>
        /// <param name="message">Optional success message</param>
        /// <returns>A successful API response</returns>
        public static ApiResponse<T> CreateSuccess(T data, string? message = null) =>
            new ApiResponse<T> { 
                Success = true, 
                Data = data, 
                Message = message 
            };

        /// <summary>
        /// Creates a failure response
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>A failure API response</returns>
        public static ApiResponse<T> CreateError(string message) =>
            new ApiResponse<T> { 
                Success = false, 
                Message = message 
            };
    }
}
