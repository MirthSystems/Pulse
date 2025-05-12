namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for filtering venues with various search criteria.
    /// </summary>
    /// <remarks>
    /// <para>This model supports multiple filtering dimensions that can be combined:</para>
    /// <para>- Location-based filtering (address + radius)</para>
    /// <para>- Text-based filtering (name, description)</para>
    /// <para>- Operational filtering (open at specific times)</para>
    /// <para>- Special availability filtering</para>
    /// <para>All filters are optional and can be combined for advanced searching.</para>
    /// </remarks>
    public class GetVenuesRequest
    {
        /// <summary>
        /// Gets or sets the page number (1-based) for pagination.
        /// </summary>
        /// <example>1</example>
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page for pagination.
        /// </summary>
        /// <example>20</example>
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the search text to filter venues by name or description.
        /// </summary>
        /// <remarks>
        /// <para>This performs a case-insensitive search on venue name and description fields.</para>
        /// <para>Partial matches are supported (e.g., "coffee" will match "Coffee Shop").</para>
        /// </remarks>
        /// <example>coffee</example>
        [StringLength(100)]
        public string? SearchText { get; set; }

        /// <summary>
        /// Gets or sets the address to search around for nearby venues.
        /// </summary>
        /// <remarks>
        /// <para>This can be any valid address format that can be geocoded.</para>
        /// <para>Examples: "123 Main St, Chicago, IL" or "Chicago, IL" or "60601"</para>
        /// <para>Must be used in conjunction with RadiusInMiles.</para>
        /// </remarks>
        /// <example>Chicago, IL</example>
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the search radius in miles around the specified address.
        /// </summary>
        /// <remarks>
        /// <para>Only applicable when Address is provided.</para>
        /// <para>Defaults to 5 miles if not specified.</para>
        /// </remarks>
        /// <example>5</example>
        [Range(0.1, 100, ErrorMessage = "Radius must be between 0.1 and 100 miles.")]
        public double? RadiusInMiles { get; set; }

        /// <summary>
        /// Gets or sets the day of week to check for open venues.
        /// </summary>
        /// <remarks>
        /// <para>Only returns venues that are open on this day.</para>
        /// <para>If TimeOfDay is also specified, only returns venues open at that specific time.</para>
        /// </remarks>
        /// <example>1</example>
        public DayOfWeek? OpenOnDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the time of day to check for open venues (format: HH:mm).
        /// </summary>
        /// <remarks>
        /// <para>Only returns venues that are open at this time on the specified day.</para>
        /// <para>Must be used in conjunction with OpenOnDayOfWeek.</para>
        /// <para>Format: 24-hour time format (HH:mm)</para>
        /// </remarks>
        /// <example>18:30</example>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in HH:mm format")]
        public string? TimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to only include venues with active specials.
        /// </summary>
        /// <remarks>
        /// <para>When true, only venues with at least one currently active special are returned.</para>
        /// <para>Useful for finding venues with ongoing promotions or events.</para>
        /// </remarks>
        /// <example>true</example>
        public bool? HasActiveSpecials { get; set; }

        /// <summary>
        /// Gets or sets the type of specials to filter venues by.
        /// </summary>
        /// <remarks>
        /// <para>Only returns venues that have specials of the specified type.</para>
        /// <para>Can be combined with HasActiveSpecials to find venues with active specials of a specific type.</para>
        /// </remarks>
        /// <example>1</example>
        public int? SpecialTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include detailed address information in the results.
        /// </summary>
        /// <remarks>
        /// <para>When true, the full address details are included in the response.</para>
        /// <para>When false, only basic venue information is returned.</para>
        /// </remarks>
        /// <example>true</example>
        public bool IncludeAddressDetails { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to include business hours in the results.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue's operating hours are included in the response.</para>
        /// <para>This can increase response size but provides more complete venue information.</para>
        /// </remarks>
        /// <example>false</example>
        public bool IncludeBusinessHours { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the sort order for venue results.
        /// </summary>
        /// <remarks>
        /// <para>Options:</para>
        /// <para>0 = Default (Name ascending)</para>
        /// <para>1 = Name (A-Z)</para>
        /// <para>2 = Name (Z-A)</para>
        /// <para>3 = Distance (Near to Far, when Address is provided)</para>
        /// <para>4 = Most Specials</para>
        /// </remarks>
        /// <example>0</example>
        [Range(0, 4, ErrorMessage = "SortOrder must be between 0 and 4.")]
        public int SortOrder { get; set; } = 0;
    }
}