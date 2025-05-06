namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;

    /// <summary>
    /// Repository interface for operating schedule entities, extending the base repository with schedule-specific operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines operating schedule-specific data access operations beyond the basic CRUD operations.</para>
    /// <para>It provides methods for retrieving schedules by venue and determining if venues are open at specific times.</para>
    /// <para>Implementations handle the actual data access logic and database interactions.</para>
    /// </remarks>
    public interface IOperatingScheduleRepository : IRepository<OperatingSchedule>
    {
        /// <summary>
        /// Gets all operating schedules for a specific venue.
        /// </summary>
        /// <param name="venueId">The primary key of the venue.</param>
        /// <returns>A list of operating schedules for the venue, ordered by day of week.</returns>
        /// <remarks>
        /// <para>This method retrieves all operating schedules for a venue, ordered from Sunday to Saturday.</para>
        /// <para>A complete set typically includes seven entries, one for each day of the week.</para>
        /// <para>The ordering ensures consistent presentation from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        Task<List<OperatingSchedule>> GetSchedulesByVenueIdAsync(long venueId);

        /// <summary>
        /// Determines if a venue is open at a specific day and time.
        /// </summary>
        /// <param name="venueId">The primary key of the venue.</param>
        /// <param name="dayOfWeek">The day of the week to check.</param>
        /// <param name="currentTime">The time of day to check.</param>
        /// <returns>True if the venue is open at the specified day and time; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method checks if a venue is open by evaluating its operating schedule for the specified day.</para>
        /// <para>It handles various cases including:</para>
        /// <para>- Days when the venue is marked as closed</para>
        /// <para>- Normal operating hours where open time is before close time</para>
        /// <para>- Operating hours that span midnight (close time is earlier than open time)</para>
        /// <para>This is useful for filtering venues that are currently open and displaying open/closed indicators.</para>
        /// </remarks>
        Task<bool> IsVenueOpenAsync(long venueId, DayOfWeek dayOfWeek, TimeOnly currentTime);
    }
}
