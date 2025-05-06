namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;

    /// <summary>
    /// Repository for managing venue operating schedule entities in the database.
    /// </summary>
    /// <remarks>
    /// <para>This repository extends the base repository with operating schedule-specific query methods.</para>
    /// <para>It handles schedule-related data access operations including:</para>
    /// <para>- Retrieving operating schedules by venue</para>
    /// <para>- Determining if a venue is currently open based on day and time</para>
    /// </remarks>
    public class OperatingScheduleRepository : Repository<OperatingSchedule>, IOperatingScheduleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingScheduleRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks>
        /// Passes the database context to the base repository constructor.
        /// </remarks>
        public OperatingScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all operating schedules for a specific venue.
        /// </summary>
        /// <param name="venueId">The primary key of the venue.</param>
        /// <returns>A list of operating schedules for the venue, ordered by day of week.</returns>
        /// <remarks>
        /// <para>This method retrieves all operating schedules for a venue, ordered by day of week.</para>
        /// <para>The ordering ensures a consistent presentation of days from Sunday to Saturday.</para>
        /// <para>A venue typically has seven entries, one for each day of the week.</para>
        /// </remarks>
        public async Task<List<OperatingSchedule>> GetSchedulesByVenueIdAsync(long venueId)
        {
            return await _context.OperatingSchedules
                .Where(os => os.VenueId == venueId)
                .OrderBy(os => os.DayOfWeek)
                .ToListAsync();
        }

        /// <summary>
        /// Determines if a venue is currently open based on the day of week and time.
        /// </summary>
        /// <param name="venueId">The primary key of the venue.</param>
        /// <param name="dayOfWeek">The day of the week to check.</param>
        /// <param name="currentTime">The time to check.</param>
        /// <returns>True if the venue is open at the specified day and time; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method checks if a venue is open at a specific day and time based on its operating schedule.</para>
        /// <para>If no schedule is found for the specified day, or the schedule is marked as closed, the venue is considered closed.</para>
        /// <para>The logic handles cases where the closing time is after midnight by checking if:</para>
        /// <para>- The closing time is earlier than the opening time (indicating crossing midnight)</para>
        /// <para>- The current time is either after opening time or before closing time (indicating within the range spanning midnight)</para>
        /// </remarks>
        public async Task<bool> IsVenueOpenAsync(long venueId, DayOfWeek dayOfWeek, TimeOnly currentTime)
        {
            var schedule = await _context.OperatingSchedules
                .FirstOrDefaultAsync(os => os.VenueId == venueId && os.DayOfWeek == dayOfWeek);

            if (schedule == null || schedule.IsClosed)
            {
                return false;
            }

            var timeOpen = new TimeOnly(schedule.TimeOfOpen.Hour, schedule.TimeOfOpen.Minute);
            var timeClose = new TimeOnly(schedule.TimeOfClose.Hour, schedule.TimeOfClose.Minute);

            // Handle cases where closing time is after midnight
            if (timeClose < timeOpen)
            {
                return currentTime >= timeOpen || currentTime <= timeClose;
            }
            else
            {
                return currentTime >= timeOpen && currentTime <= timeClose;
            }
        }
    }
}
