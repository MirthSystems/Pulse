namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;

    public interface IOperatingScheduleRepository : IRepository<OperatingSchedule>
    {
        /// <summary>
        /// Gets all operating schedules for a venue
        /// </summary>
        Task<List<OperatingSchedule>> GetSchedulesByVenueIdAsync(long venueId);

        /// <summary>
        /// Determines if a venue is currently open
        /// </summary>
        Task<bool> IsVenueOpenAsync(long venueId, DayOfWeek dayOfWeek, TimeOnly currentTime);
    }
}
