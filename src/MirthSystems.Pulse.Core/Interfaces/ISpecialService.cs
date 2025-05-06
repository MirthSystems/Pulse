namespace MirthSystems.Pulse.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;

    /// <summary>
    /// Service interface for special promotion-related business operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines the contract for special-related business logic.</para>
    /// <para>It provides methods for retrieving, creating, updating, and deleting special promotions.</para>
    /// <para>It also includes methods for checking if specials are currently active based on their scheduling.</para>
    /// <para>Implementations handle the coordination between repositories, external services, and mapping to response models.</para>
    /// </remarks>
    public interface ISpecialService
    {
        /// <summary>
        /// Gets a filtered and paged list of specials based on search criteria.
        /// </summary>
        /// <param name="request">The request containing filter, search, and pagination parameters.</param>
        /// <returns>A paged API response containing special list items and pagination metadata.</returns>
        /// <remarks>
        /// <para>This method provides advanced search and filtering capabilities including:</para>
        /// <para>- Location-based search within a specified radius</para>
        /// <para>- Text search in special content and venue names</para>
        /// <para>- Filtering by special type (food, drink, entertainment)</para>
        /// <para>- Filtering by current activity status</para>
        /// <para>- Filtering by venue</para>
        /// <para>- Pagination for efficient data retrieval</para>
        /// </remarks>
        Task<PagedApiResponse<SpecialListItem>> GetSpecialsAsync(GetSpecialsRequest request);

        /// <summary>
        /// Gets detailed information about a specific special by ID.
        /// </summary>
        /// <param name="id">The ID of the special to retrieve.</param>
        /// <returns>Detailed special information if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method retrieves comprehensive information about a special including:</para>
        /// <para>- Content and type</para>
        /// <para>- Timing details (start/end times, expiration)</para>
        /// <para>- Recurrence information</para>
        /// <para>- Current activity status</para>
        /// <para>- Associated venue information</para>
        /// <para>The response is suitable for special detail pages and management interfaces.</para>
        /// </remarks>
        Task<SpecialDetail?> GetSpecialByIdAsync(string id);

        /// <summary>
        /// Creates a new special promotion.
        /// </summary>
        /// <param name="request">The special creation request containing special details.</param>
        /// <param name="userId">The ID of the user creating the special.</param>
        /// <returns>The created special with generated IDs and metadata.</returns>
        /// <remarks>
        /// <para>This method handles the complete special creation process including:</para>
        /// <para>- Validating the associated venue exists</para>
        /// <para>- Creating the special record</para>
        /// <para>- Setting creation metadata (timestamp, user)</para>
        /// <para>- Determining the initial activity status based on scheduling information</para>
        /// </remarks>
        Task<SpecialDetail> CreateSpecialAsync(CreateSpecialRequest request, string userId);

        /// <summary>
        /// Updates an existing special promotion.
        /// </summary>
        /// <param name="id">The ID of the special to update.</param>
        /// <param name="request">The special update request containing the new special details.</param>
        /// <param name="userId">The ID of the user updating the special.</param>
        /// <returns>The updated special details.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a special with the specified ID is not found.</exception>
        /// <remarks>
        /// <para>This method handles the complete special update process including:</para>
        /// <para>- Updating the special record with new values</para>
        /// <para>- Setting update metadata (timestamp, user)</para>
        /// <para>- Recalculating the activity status based on updated scheduling information</para>
        /// </remarks>
        Task<SpecialDetail> UpdateSpecialAsync(string id, UpdateSpecialRequest request, string userId);

        /// <summary>
        /// Soft-deletes a special promotion.
        /// </summary>
        /// <param name="id">The ID of the special to delete.</param>
        /// <param name="userId">The ID of the user deleting the special.</param>
        /// <returns>True if the special was found and deleted; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs a soft delete, which means:</para>
        /// <para>- The special is marked as deleted but remains in the database</para>
        /// <para>- The special will be excluded from normal queries</para>
        /// <para>- The deletion is recorded with a timestamp and user ID</para>
        /// </remarks>
        Task<bool> DeleteSpecialAsync(string id, string userId);

        /// <summary>
        /// Determines if a special is currently running at a specific point in time.
        /// </summary>
        /// <param name="specialId">The ID of the special to check.</param>
        /// <param name="referenceTime">The time to check against, or null to use the current time.</param>
        /// <returns>True if the special is currently active; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method determines whether a special is active by evaluating:</para>
        /// <para>- Start date and time</para>
        /// <para>- End time if specified</para>
        /// <para>- Expiration date if specified</para>
        /// <para>- Recurrence pattern (CRON schedule) if the special is recurring</para>
        /// <para>This is useful for filtering active specials and displaying status indicators.</para>
        /// </remarks>
        Task<bool> IsSpecialCurrentlyRunningAsync(string specialId, DateTimeOffset? referenceTime = null);
    }
}
