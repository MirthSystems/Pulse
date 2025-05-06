namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for searching and filtering specials in the system.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the parameters for searching and filtering specials.</para>
    /// <para>It includes options for location-based searches, text search, time-based filtering, and pagination.</para>
    /// <para>Used primarily for API endpoints that retrieve lists of specials.</para>
    /// </remarks>
    public class GetSpecialsRequest
    {
        /// <summary>
        /// Gets or sets the address to search near.
        /// </summary>
        /// <remarks>
        /// <para>This address will be geocoded to determine the center point for proximity searches.</para>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St, Chicago, IL"</para>
        /// <para>- "Times Square, New York, NY"</para>
        /// <para>- "90210" (ZIP code only)</para>
        /// </remarks>
        [Required]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the search radius in miles.
        /// </summary>
        /// <remarks>
        /// <para>Specifies the radius (in miles) to search around the geocoded address.</para>
        /// <para>Default value is 5 miles.</para>
        /// <para>Examples: 1 (walking distance), 5 (neighborhood), 25 (regional)</para>
        /// </remarks>
        [Range(0.1, 100)]
        public double Radius { get; set; } = 5;

        /// <summary>
        /// Gets or sets the date and time for which to check special availability.
        /// </summary>
        /// <remarks>
        /// <para>Format: ISO 8601 format (YYYY-MM-DDThh:mm:ssZ)</para>
        /// <para>Examples: "2023-12-15T18:30:00Z", "2024-01-01T00:00:00Z"</para>
        /// <para>If empty, the current date and time will be used.</para>
        /// </remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$", ErrorMessage = "Invalid date-time format. Use ISO 8601 format (e.g., 2025-05-05T18:30:00Z).")]
        public string SearchDateTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text to search for in special descriptions and venue names.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "happy hour" (finds all happy hour specials)</para>
        /// <para>- "pizza" (finds specials mentioning pizza)</para>
        /// <para>- "jazz" (finds music venues with jazz performances)</para>
        /// </remarks>
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue ID to filter specials by.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>When provided, only returns specials for the specified venue.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the special type ID to filter by.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 0: Food specials</para>
        /// <para>- 1: Drink specials</para>
        /// <para>- 2: Entertainment specials</para>
        /// </remarks>
        [Range(1, int.MaxValue, ErrorMessage = "SpecialTypeId must be a positive number.")]
        public int? SpecialTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include only currently running specials.
        /// </summary>
        /// <remarks>
        /// <para>True: Only return specials that are active at SearchDateTime.</para>
        /// <para>False: Include all specials regardless of their active status.</para>
        /// <para>Default is true.</para>
        /// </remarks>
        public bool? IsCurrentlyRunning { get; set; } = true;

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        /// <remarks>
        /// <para>This is a 1-based index (first page is 1, not 0).</para>
        /// <para>Default is 1.</para>
        /// </remarks>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page for pagination.
        /// </summary>
        /// <remarks>
        /// <para>Defines how many specials are returned per page.</para>
        /// <para>Default is 20.</para>
        /// <para>Maximum allowed value is 100.</para>
        /// </remarks>
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}
