namespace Pulse.Core.Contracts
{
    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Repository interface for OperatingSchedule-specific operations
    /// </summary>
    public interface IOperatingScheduleRepository : IRepository<OperatingSchedule, long>
    {
        /// <summary>
        /// Gets all operating schedules for a specific venue
        /// </summary>
        /// <param name="venueId">Venue ID</param>
        /// <returns>Collection of operating schedules</returns>
        Task<IEnumerable<OperatingSchedule>> GetByVenueIdAsync(long venueId);

        /// <summary>
        /// Gets the operating schedule for a specific day at a venue
        /// </summary>
        /// <param name="venueId">Venue ID</param>
        /// <param name="dayOfWeek">Day of week</param>
        /// <returns>Operating schedule if found, null otherwise</returns>
        Task<OperatingSchedule?> GetByVenueAndDayAsync(long venueId, DayOfWeek dayOfWeek);

        /// <summary>
        /// Updates all operating schedules for a venue
        /// </summary>
        /// <param name="venueId">Venue ID</param>
        /// <param name="schedules">Operating schedules</param>
        /// <param name="userId">ID of the user performing the action</param>
        Task UpdateVenueSchedulesAsync(long venueId, IEnumerable<OperatingSchedule> schedules, string userId);
    }
}
