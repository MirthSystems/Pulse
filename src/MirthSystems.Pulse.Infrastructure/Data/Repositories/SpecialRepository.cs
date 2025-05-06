namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Cronos;

    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Enums;
    using MirthSystems.Pulse.Core.Interfaces;

    using NetTopologySuite.Geometries;

    using NodaTime;

    public class SpecialRepository : Repository<Special>, ISpecialRepository
    {
        public SpecialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<Special> GetAll()
        {
            return _dbSet.Where(s => !s.IsDeleted);
        }

        public async Task<Special?> GetSpecialWithVenueAsync(long id)
        {
            return await _context.Specials
                .Include(s => s.Venue)
                .ThenInclude(v => v.Address)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<(List<Special> specials, int totalCount)> GetPagedSpecialsAsync(
            int page,
            int pageSize,
            Point? location = null,
            double? distanceInMeters = null,
            string? searchTerm = null,
            SpecialTypes? type = null,
            bool includeExpired = false)
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var today = LocalDate.FromDateTime(DateTime.Today);

            var query = _context.Specials
                .Include(s => s.Venue)
                .ThenInclude(v => v.Address)
                .Where(s => !s.IsDeleted && !s.Venue.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(s => s.Content.ToLower().Contains(searchTerm) ||
                                        s.Venue.Name.ToLower().Contains(searchTerm));
            }

            if (type.HasValue)
            {
                query = query.Where(s => s.Type == type.Value);
            }

            if (location != null && distanceInMeters.HasValue)
            {
                query = query.Where(s => s.Venue.Address.Location.Distance(location) <= distanceInMeters.Value);
            }

            if (!includeExpired)
            {
                query = query.Where(s => s.ExpirationDate == null || s.ExpirationDate >= today);
            }

            query = query.OrderByDescending(s => s.CreatedAt)
                         .ThenBy(s => s.Venue.Name);

            var totalCount = await query.CountAsync();

            var specials = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (specials, totalCount);
        }

        public async Task<List<Special>> GetSpecialsByVenueIdAsync(long venueId)
        {
            return await _context.Specials
                .Where(s => s.VenueId == venueId && !s.IsDeleted)
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<bool> IsSpecialCurrentlyActiveAsync(long specialId, Instant referenceTime)
        {
            var special = await _context.Specials
                .FirstOrDefaultAsync(s => s.Id == specialId && !s.IsDeleted);

            if (special == null)
            {
                return false;
            }

            var localDateTime = referenceTime.InUtc().ToDateTimeUtc();
            var localDate = LocalDate.FromDateTime(localDateTime.Date);
            var localTime = LocalTime.FromTicksSinceMidnight(localDateTime.TimeOfDay.Ticks);

            if (special.ExpirationDate.HasValue && special.ExpirationDate.Value < localDate)
            {
                return false;
            }

            if (special.StartDate > localDate)
            {
                return false;
            }

            if (!special.IsRecurring && special.StartDate == localDate)
            {
                if (special.EndTime.HasValue)
                {
                    return localTime >= special.StartTime && localTime <= special.EndTime.Value;
                }
                else
                {
                    return localTime >= special.StartTime;
                }
            }

            if (special.IsRecurring && !string.IsNullOrEmpty(special.CronSchedule))
            {
                try
                {
                    var cronExpression = CronExpression.Parse(special.CronSchedule, CronFormat.Standard);

                    // Get the start date from special (local date)
                    var startDateTime = new DateTime(
                        special.StartDate.Year,
                        special.StartDate.Month,
                        special.StartDate.Day,
                        0, 0, 0,
                        DateTimeKind.Utc);

                    // Set up the date range for today (to check if the special occurs today)
                    var todayStart = new DateTime(
                        localDateTime.Year,
                        localDateTime.Month,
                        localDateTime.Day,
                        0, 0, 0,
                        DateTimeKind.Utc);

                    var todayEnd = todayStart.AddDays(1);

                    // Get all occurrences for today
                    var todayOccurrences = cronExpression.GetOccurrences(startDateTime, todayEnd)
                        .Where(o => o.Date == todayStart.Date)
                        .ToList();

                    // If no occurrences today, the special isn't running
                    if (todayOccurrences.Count == 0)
                    {
                        return false;
                    }

                    // check if current time is within the special's hours
                    var startTimeOnCurrentDay = new DateTime(
                        localDateTime.Year,
                        localDateTime.Month,
                        localDateTime.Day,
                        special.StartTime.Hour,
                        special.StartTime.Minute,
                        special.StartTime.Second,
                        DateTimeKind.Utc);

                    if (special.EndTime.HasValue)
                    {
                        var endTimeOnCurrentDay = new DateTime(
                            localDateTime.Year,
                            localDateTime.Month,
                            localDateTime.Day,
                            special.EndTime.Value.Hour,
                            special.EndTime.Value.Minute,
                            special.EndTime.Value.Second,
                            DateTimeKind.Utc);

                        // Handle the case where end time is after midnight
                        if (endTimeOnCurrentDay < startTimeOnCurrentDay)
                        {
                            endTimeOnCurrentDay = endTimeOnCurrentDay.AddDays(1);
                        }

                        // Check if current time is between start and end times
                        return localDateTime >= startTimeOnCurrentDay && localDateTime <= endTimeOnCurrentDay;
                    }
                    else
                    {
                        // No end time specified, just check if after start time
                        return localDateTime >= startTimeOnCurrentDay;
                    }
                }
                catch (Exception)
                {
                    // Invalid CRON expression, default to basic time range checking
                    if (special.EndTime.HasValue)
                    {
                        return localTime >= special.StartTime && localTime <= special.EndTime.Value;
                    }
                    else
                    {
                        return localTime >= special.StartTime;
                    }
                }
            }

            return false;
        }
    }
}
