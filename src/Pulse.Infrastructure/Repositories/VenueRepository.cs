namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using NetTopologySuite.Algorithm;
    using NetTopologySuite.Algorithm.Distance;
    using NetTopologySuite.Geometries;

    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;
    using Pulse.Core.Utilities;

    public class VenueRepository : Repository<Venue, long>, IVenueRepository
    {
        private static readonly Func<ApplicationDbContext, long, Task<Venue?>> _getWithAllDataQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long id) =>
                context.Venues
                    .Include(v => v.VenueType)
                    .Include(v => v.BusinessHours)
                    .Include(v => v.Specials)
                        .ThenInclude(s => s.Tags)
                            .ThenInclude(tsl => tsl.Tag)
                    .FirstOrDefault(v => v.Id == id));

        public VenueRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<IEnumerable<Venue>> FindNearbyAsync(Point location, double radiusMiles)
        {
            double radiusMeters = radiusMiles * 1609.344;

            EnsureSrid(ref location);

            return await _dbSet
                .AsNoTracking()
                .Where(v => v.Location != null && v.Location.IsWithinDistance(location, radiusMeters))
                .OrderBy(v => v.Location!.Distance(location))
                .ToListAsync();
        }

        public async Task<IEnumerable<Venue>> FindNearbyWithActiveSpecialsAsync(Point location, double radiusMiles)
        {
            double radiusMeters = radiusMiles * 1609.344;

            EnsureSrid(ref location);

            var now = _clock.GetCurrentInstant();

            var venueIds = await _dbSet
                .AsNoTracking()
                .Where(v => v.Location != null && v.Location.IsWithinDistance(location, radiusMeters))
                .Select(v => v.Id)
                .ToListAsync();

            if (!venueIds.Any())
            {
                return new List<Venue>();
            }

            var venues = await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Where(v => venueIds.Contains(v.Id))
                .Include(v => v.Specials)
                .OrderBy(v => v.Location!.Distance(location))
                .ToListAsync();

            return venues
                .Where(v => v.Specials.Any(s => SpecialHelper.IsActive(s, now)))
                .ToList();
        }

        public async Task<Venue?> GetWithVenueTypeAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(v => v.VenueType)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue?> GetWithBusinessHoursAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(v => v.BusinessHours)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue?> GetWithSpecialsAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(v => v.Specials)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue?> GetWithAllDataAsync(long id)
        {
            return await _getWithAllDataQuery(_context, id);
        }

        private static void EnsureSrid(ref Point location)
        {
            if (location.SRID == 0)
            {
                location = new Point(location.X, location.Y) { SRID = 4326 };
            }
        }
    }
}
