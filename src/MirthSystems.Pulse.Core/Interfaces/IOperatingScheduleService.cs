namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Requests;

    /// <summary>
    /// Service interface for operating schedule operations, providing venue business hours management.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines business logic operations related to venue operating schedules, including:</para>
    /// <para>- Retrieving operating schedules (individual or by venue)</para>
    /// <para>- Creating new operating schedules for venues</para>
    /// <para>- Updating existing operating schedule details</para>
    /// <para>- Deleting operating schedules</para>
    /// <para>Implementations handle data validation, database operations, and business logic.</para>
    /// </remarks>
    public interface IOperatingScheduleService
    {
        /// <summary>
        /// Retrieves an operating schedule by its ID.
        /// </summary>
        /// <param name="id">The operating schedule ID.</param>
        /// <returns>The operating schedule details if found, otherwise null.</returns>
        /// <remarks>
        /// <para>This method returns comprehensive information about an operating schedule.</para>
        /// <para>Includes venue information and day/time details.</para>
        /// </remarks>
        Task<OperatingScheduleItemExtended?> GetOperatingScheduleByIdAsync(string id);

        /// <summary>
        /// Creates a new operating schedule.
        /// </summary>
        /// <param name="request">The operating schedule creation request.</param>
        /// <param name="userId">The ID of the user creating the operating schedule.</param>
        /// <returns>The created operating schedule details.</returns>
        /// <remarks>
        /// <para>This method validates the request and persists a new operating schedule.</para>
        /// <para>Supports setting open/close times and closed days.</para>
        /// </remarks>
        Task<OperatingScheduleItemExtended> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId);

        /// <summary>
        /// Updates an existing operating schedule.
        /// </summary>
        /// <param name="id">The operating schedule ID.</param>
        /// <param name="request">The operating schedule update request.</param>
        /// <param name="userId">The ID of the user updating the operating schedule.</param>
        /// <returns>The updated operating schedule details.</returns>
        /// <remarks>
        /// <para>This method validates the request and updates an existing operating schedule.</para>
        /// <para>Throws KeyNotFoundException if the operating schedule doesn't exist.</para>
        /// </remarks>
        Task<OperatingScheduleItemExtended> UpdateOperatingScheduleAsync(string id, UpdateOperatingScheduleRequest request, string userId);

        /// <summary>
        /// Deletes an operating schedule.
        /// </summary>
        /// <param name="id">The operating schedule ID.</param>
        /// <param name="userId">The ID of the user deleting the operating schedule.</param>
        /// <returns>True if successfully deleted, otherwise false.</returns>
        /// <remarks>
        /// <para>This method removes the operating schedule from the database.</para>
        /// <para>Unlike venues and specials, operating schedules use physical deletion rather than soft deletion.</para>
        /// </remarks>
        Task<bool> DeleteOperatingScheduleAsync(string id, string userId);
    }
}
