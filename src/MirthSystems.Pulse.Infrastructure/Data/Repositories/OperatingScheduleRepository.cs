namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;

    public class OperatingScheduleRepository : Repository<OperatingSchedule>, IOperatingScheduleRepository
    {
        public OperatingScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<OperatingSchedule>> GetSchedulesByVenueIdAsync(long venueId)
        {
            return await _context.OperatingSchedules
                .Where(os => os.VenueId == venueId)
                .OrderBy(os => os.DayOfWeek)
                .ToListAsync();
        }

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
