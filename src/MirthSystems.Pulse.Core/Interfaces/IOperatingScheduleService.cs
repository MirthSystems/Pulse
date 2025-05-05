namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models.Requests;

    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Responses;

    /// <summary>
    /// Service for managing operating schedules
    /// </summary>
    public interface IOperatingScheduleService
    {
        /// <summary>
        /// Gets an operating schedule by ID
        /// </summary>
        /// <param name="id">The operating schedule ID</param>
        /// <returns>The operating schedule or null if not found</returns>
        Task<OperatingScheduleDetail?> GetOperatingScheduleByIdAsync(long id);

        /// <summary>
        /// Creates a new operating schedule
        /// </summary>
        /// <param name="request">The creation request</param>
        /// <param name="userId">The ID of the user creating the schedule</param>
        /// <returns>The created operating schedule</returns>
        Task<OperatingScheduleDetail> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId);

        /// <summary>
        /// Updates an existing operating schedule
        /// </summary>
        /// <param name="id">The operating schedule ID</param>
        /// <param name="request">The update request</param>
        /// <param name="userId">The ID of the user updating the schedule</param>
        /// <returns>The updated operating schedule</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the operating schedule is not found</exception>
        Task<OperatingScheduleDetail> UpdateOperatingScheduleAsync(long id, UpdateOperatingScheduleRequest request, string userId);

        /// <summary>
        /// Deletes an operating schedule
        /// </summary>
        /// <param name="id">The operating schedule ID</param>
        /// <param name="userId">The ID of the user deleting the schedule</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteOperatingScheduleAsync(long id, string userId);

        /// <summary>
        /// Gets operating schedules for a venue
        /// </summary>
        /// <param name="venueId">The venue ID</param>
        /// <returns>List of operating schedules for the venue</returns>
        Task<List<OperatingScheduleDetail>> GetVenueOperatingSchedulesAsync(long venueId);

        /// <summary>
        /// Creates multiple operating schedules for a venue
        /// </summary>
        /// <param name="venueId">The venue ID</param>
        /// <param name="requests">The creation requests</param>
        /// <param name="userId">The ID of the user creating the schedules</param>
        /// <returns>True if successful</returns>
        Task<bool> CreateOperatingSchedulesForVenueAsync(long venueId, List<CreateOperatingScheduleRequest> requests, string userId);
    }
}
