namespace MirthSystems.Pulse.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;

    /// <summary>
    /// Service interface for special promotions and events operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines business logic operations related to venue specials, including:</para>
    /// <para>- Retrieving specials (individual or paginated listings with filtering)</para>
    /// <para>- Creating new specials with scheduling information</para>
    /// <para>- Updating existing special details</para>
    /// <para>- Deleting specials (soft delete)</para>
    /// <para>- Determining if a special is currently active</para>
    /// <para>Implementations handle data validation, database operations, and scheduling logic.</para>
    /// </remarks>
    public interface ISpecialService
    {
        /// <summary>
        /// Retrieves a filtered, paginated list of specials.
        /// </summary>
        /// <param name="request">The special search and filter request parameters.</param>
        /// <returns>A paginated list of special list items.</returns>
        /// <remarks>
        /// <para>This method returns specials with filtering based on:</para>
        /// <para>- Geographic location and radius</para>
        /// <para>- Special type (food, drink, entertainment)</para>
        /// <para>- Whether the special is currently running</para>
        /// <para>- Text search terms</para>
        /// <para>- Venue ID filtering</para>
        /// </remarks>
        Task<PagedResult<SpecialListItem>> GetSpecialsAsync(GetSpecialsRequest request);

        /// <summary>
        /// Retrieves a special by its ID.
        /// </summary>
        /// <param name="id">The special ID.</param>
        /// <returns>The special details if found, otherwise null.</returns>
        /// <remarks>
        /// <para>This method returns comprehensive information about a special.</para>
        /// <para>Includes venue information and current running status.</para>
        /// </remarks>
        Task<SpecialDetail?> GetSpecialByIdAsync(string id);

        /// <summary>
        /// Creates a new special.
        /// </summary>
        /// <param name="request">The special creation request.</param>
        /// <param name="userId">The ID of the user creating the special.</param>
        /// <returns>The created special details.</returns>
        /// <remarks>
        /// <para>This method validates the request and persists a new special.</para>
        /// <para>Supports one-time and recurring specials with various scheduling options.</para>
        /// </remarks>
        Task<SpecialDetail> CreateSpecialAsync(CreateSpecialRequest request, string userId);

        /// <summary>
        /// Updates an existing special.
        /// </summary>
        /// <param name="id">The special ID.</param>
        /// <param name="request">The special update request.</param>
        /// <param name="userId">The ID of the user updating the special.</param>
        /// <returns>The updated special details.</returns>
        /// <remarks>
        /// <para>This method validates the request and updates an existing special.</para>
        /// <para>Throws KeyNotFoundException if the special doesn't exist.</para>
        /// </remarks>
        Task<SpecialDetail> UpdateSpecialAsync(string id, UpdateSpecialRequest request, string userId);

        /// <summary>
        /// Deletes a special (soft delete).
        /// </summary>
        /// <param name="id">The special ID.</param>
        /// <param name="userId">The ID of the user deleting the special.</param>
        /// <returns>True if successfully deleted, otherwise false.</returns>
        /// <remarks>
        /// <para>This method performs a soft delete, marking the special as deleted in the database.</para>
        /// <para>The special record remains in the database for audit purposes.</para>
        /// </remarks>
        Task<bool> DeleteSpecialAsync(string id, string userId);

        /// <summary>
        /// Checks if a special is currently running at a specific time.
        /// </summary>
        /// <param name="specialId">The special ID.</param>
        /// <param name="referenceTime">The reference time to check against, or null for current time.</param>
        /// <returns>True if the special is currently active, otherwise false.</returns>
        /// <remarks>
        /// <para>This method determines if a special is active based on:</para>
        /// <para>- Start date and time</para>
        /// <para>- End time (if specified)</para>
        /// <para>- Expiration date (if specified)</para>
        /// <para>- Recurrence schedule for recurring specials</para>
        /// </remarks>
        Task<bool> IsSpecialCurrentlyRunningAsync(string specialId, DateTimeOffset? referenceTime = null);
    }
}
