namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models.Requests;

    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Responses;

    /// <summary>
    /// Service interface for operating schedule-related business operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines the contract for operating schedule-related business logic.</para>
    /// <para>It provides methods for retrieving, creating, updating, and deleting operating schedules.</para>
    /// <para>It also includes methods for managing multiple operating schedules for a venue.</para>
    /// <para>Implementations handle the coordination between repositories and mapping to response models.</para>
    /// </remarks>
    public interface IOperatingScheduleService
    {
        /// <summary>
        /// Gets detailed information about a specific operating schedule by ID.
        /// </summary>
        /// <param name="id">The ID of the operating schedule to retrieve.</param>
        /// <returns>Detailed operating schedule information if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method retrieves comprehensive information about an operating schedule including:</para>
        /// <para>- Day of week</para>
        /// <para>- Opening and closing times</para>
        /// <para>- Whether the venue is closed on this day</para>
        /// <para>- Associated venue information</para>
        /// </remarks>
        Task<OperatingScheduleDetail?> GetOperatingScheduleByIdAsync(string id);

        /// <summary>
        /// Creates a new operating schedule.
        /// </summary>
        /// <param name="request">The operating schedule creation request.</param>
        /// <param name="userId">The ID of the user creating the operating schedule.</param>
        /// <returns>The created operating schedule with generated ID.</returns>
        /// <remarks>
        /// <para>This method handles the complete operating schedule creation process including:</para>
        /// <para>- Validating the associated venue exists</para>
        /// <para>- Creating the operating schedule record</para>
        /// <para>- Converting time strings to appropriate time representations</para>
        /// </remarks>
        Task<OperatingScheduleDetail> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId);

        /// <summary>
        /// Updates an existing operating schedule.
        /// </summary>
        /// <param name="id">The ID of the operating schedule to update.</param>
        /// <param name="request">The operating schedule update request.</param>
        /// <param name="userId">The ID of the user updating the operating schedule.</param>
        /// <returns>The updated operating schedule details.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when an operating schedule with the specified ID is not found.</exception>
        /// <remarks>
        /// <para>This method handles the operating schedule update process including:</para>
        /// <para>- Validating the operating schedule exists</para>
        /// <para>- Updating the open time, close time, and closed status</para>
        /// <para>- Converting time strings to appropriate time representations</para>
        /// </remarks>
        Task<OperatingScheduleDetail> UpdateOperatingScheduleAsync(string id, UpdateOperatingScheduleRequest request, string userId);

        /// <summary>
        /// Deletes an operating schedule.
        /// </summary>
        /// <param name="id">The ID of the operating schedule to delete.</param>
        /// <param name="userId">The ID of the user deleting the operating schedule.</param>
        /// <returns>True if the operating schedule was found and deleted; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs a physical delete of the operating schedule.</para>
        /// <para>Note that deleting an operating schedule may create incomplete business hours for a venue.</para>
        /// <para>Consider using CreateOperatingSchedulesForVenueAsync to replace the full set of schedules instead.</para>
        /// </remarks>
        Task<bool> DeleteOperatingScheduleAsync(string id, string userId);

        /// <summary>
        /// Gets all operating schedules for a specific venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue.</param>
        /// <returns>A list of operating schedule details for the venue.</returns>
        /// <remarks>
        /// <para>This method retrieves all operating schedules for a venue, ordered by day of week.</para>
        /// <para>A complete set typically includes seven entries, one for each day of the week.</para>
        /// <para>Each entry includes opening time, closing time, and closed status for that day.</para>
        /// </remarks>
        Task<List<OperatingScheduleDetail>> GetVenueOperatingSchedulesAsync(string venueId);

        /// <summary>
        /// Creates a complete set of operating schedules for a venue, replacing any existing schedules.
        /// </summary>
        /// <param name="venueId">The ID of the venue.</param>
        /// <param name="requests">The list of operating schedule creation requests, one for each day.</param>
        /// <param name="userId">The ID of the user creating the operating schedules.</param>
        /// <returns>True if the operating schedules were created successfully; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method handles the batch creation process for a venue's complete operating schedule:</para>
        /// <para>- Deletes any existing operating schedules for the venue</para>
        /// <para>- Creates new operating schedules from the provided requests</para>
        /// <para>- Validates that all requests are for the specified venue</para>
        /// <para>This approach ensures consistency by replacing the entire schedule set in a single operation.</para>
        /// </remarks>
        Task<bool> CreateOperatingSchedulesForVenueAsync(string venueId, List<CreateOperatingScheduleRequest> requests, string userId);
    }
}
