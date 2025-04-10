namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class OperatingScheduleRepository : Repository<OperatingSchedule, long>, IOperatingScheduleRepository
    {
        public OperatingScheduleRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<IEnumerable<OperatingSchedule>> GetByVenueIdAsync(long venueId)
        {
            return await _dbSet
                .Where(os => os.VenueId == venueId)
                .OrderBy(os => os.DayOfWeek)
                .ToListAsync();
        }

        public async Task<OperatingSchedule?> GetByVenueAndDayAsync(long venueId, DayOfWeek dayOfWeek)
        {
            return await _dbSet
                .FirstOrDefaultAsync(os => os.VenueId == venueId && os.DayOfWeek == dayOfWeek);
        }

        public async Task UpdateVenueSchedulesAsync(long venueId, IEnumerable<OperatingSchedule> schedules, string userId)
        {
            // Get existing schedules
            var existingSchedules = await _dbSet
                .Where(os => os.VenueId == venueId)
                .ToListAsync();

            // Remove all existing schedules
            foreach (var schedule in existingSchedules)
            {
                _dbSet.Remove(schedule);
            }

            // Add new schedules
            foreach (var schedule in schedules)
            {
                // Ensure schedule belongs to the right venue
                schedule.VenueId = venueId;

                // Set audit fields
                schedule.CreatedAt = _clock.GetCurrentInstant();
                schedule.CreatedByUserId = userId;

                await _dbSet.AddAsync(schedule);
            }
        }
    }
}
