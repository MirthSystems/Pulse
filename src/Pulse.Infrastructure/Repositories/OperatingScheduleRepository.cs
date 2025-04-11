namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class OperatingScheduleRepository : Repository<OperatingSchedule, long>, IOperatingScheduleRepository
    {
        private static readonly Func<ApplicationDbContext, long, Task<List<OperatingSchedule>>> _getByVenueIdQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long venueId) =>
                context.BusinessHours
                    .AsNoTracking()
                    .Where(os => os.VenueId == venueId)
                    .OrderBy(os => os.DayOfWeek)
                    .ToList());

        private static readonly Func<ApplicationDbContext, long, DayOfWeek, Task<OperatingSchedule?>> _getByVenueAndDayQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long venueId, DayOfWeek dayOfWeek) =>
                context.BusinessHours
                    .AsNoTracking()
                    .FirstOrDefault(os => os.VenueId == venueId && os.DayOfWeek == dayOfWeek));

        public OperatingScheduleRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<IEnumerable<OperatingSchedule>> GetByVenueIdAsync(long venueId)
        {
            return await _getByVenueIdQuery(_context, venueId);
        }

        public async Task<OperatingSchedule?> GetByVenueAndDayAsync(long venueId, DayOfWeek dayOfWeek)
        {
            return await _getByVenueAndDayQuery(_context, venueId, dayOfWeek);
        }

        public async Task UpdateVenueSchedulesAsync(long venueId, IEnumerable<OperatingSchedule> schedules, string userId)
        {
            var existingSchedules = await _dbSet
                .Where(os => os.VenueId == venueId)
                .ToListAsync();

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _dbSet.RemoveRange(existingSchedules);

                var newSchedules = schedules.ToList();
                foreach (var schedule in newSchedules)
                {
                    schedule.VenueId = venueId;

                    schedule.CreatedAt = _clock.GetCurrentInstant();
                    schedule.CreatedByUserId = userId;
                }

                await _dbSet.AddRangeAsync(newSchedules);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
