namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Query parameters for filtering specials
    /// </summary>
    public class GetSpecialsRequest
    {
        /// <summary>
        /// The address to search around
        /// </summary>
        /// <remarks>e.g. "123 Main St, Chicago, IL"</remarks>
        [Required]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// The search radius in miles (default: 5)
        /// </summary>
        /// <remarks>e.g. 10</remarks>
        [Range(0.1, 100)]
        public double Radius { get; set; } = 5;

        /// <summary>
        /// The date and time to check if specials are running (default: now)
        /// </summary>
        /// <remarks>e.g. "2025-05-05T18:30:00Z"</remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$", ErrorMessage = "Invalid date-time format. Use ISO 8601 format (e.g., 2025-05-05T18:30:00Z).")]
        public string? SearchDateTime { get; set; }

        /// <summary>
        /// Search term for filtering specials by content or venue name
        /// </summary>
        /// <remarks>e.g. "Happy Hour" or "Burger"</remarks>
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
        public string? SearchTerm { get; set; }

        /// <summary>
        /// The venue ID to filter by, if applicable
        /// </summary>
        /// <remarks>e.g. 5</remarks>
        [Range(1, long.MaxValue, ErrorMessage = "VenueId must be a positive number.")]
        public long? VenueId { get; set; }

        /// <summary>
        /// Filter by type of special
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        [Range(1, int.MaxValue, ErrorMessage = "SpecialTypeId must be a positive number.")]
        public int? SpecialTypeId { get; set; }

        /// <summary>
        /// Whether to only return currently running specials
        /// </summary>
        /// <remarks>e.g. true</remarks>
        public bool? IsCurrentlyRunning { get; set; } = true;

        /// <summary>
        /// Page number for pagination (default: 1)
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page (default: 20, max: 100)
        /// </summary>
        /// <remarks>e.g. 20</remarks>
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}
