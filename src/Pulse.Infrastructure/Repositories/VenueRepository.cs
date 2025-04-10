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
        public VenueRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<IEnumerable<Venue>> FindNearbyAsync(Point location, double radiusMiles)
        {
            // Convert miles to meters for the spatial query
            double radiusMeters = radiusMiles * 1609.344;

            // Ensure SRID is set to 4326 (WGS84)
            if (location.SRID == 0)
            {
                location = new Point(location.X, location.Y) { SRID = 4326 };
            }

            // Use NetTopologySuite's spatial methods that translate to PostGIS functions
            return await _dbSet
                .Where(v => v.Location != null && v.Location.IsWithinDistance(location, radiusMeters))
                .OrderBy(v => v.Location!.Distance(location))
                .ToListAsync();
        }

        public async Task<IEnumerable<Venue>> FindNearbyWithActiveSpecialsAsync(Point location, double radiusMiles)
        {
            // Convert miles to meters for the spatial query
            double radiusMeters = radiusMiles * 1609.344;

            // Ensure SRID is set to 4326 (WGS84)
            if (location.SRID == 0)
            {
                location = new Point(location.X, location.Y) { SRID = 4326 };
            }

            // First, get venues with spatial filter using NTS methods
            var venues = await _dbSet
                .Where(v => v.Location != null && v.Location.IsWithinDistance(location, radiusMeters))
                .OrderBy(v => v.Location!.Distance(location))
                .Include(v => v.Specials)
                .ToListAsync();

            // Get current instant
            var now = _clock.GetCurrentInstant();

            // Filter for active specials in memory using the helper utility
            return venues
                .Where(v => v.Specials.Any(s => SpecialHelper.IsActive(s, now)))
                .ToList();
        }

        public async Task<Venue?> GetWithVenueTypeAsync(long id)
        {
            return await _dbSet
                .Include(v => v.VenueType)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue?> GetWithBusinessHoursAsync(long id)
        {
            return await _dbSet
                .Include(v => v.BusinessHours)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue?> GetWithSpecialsAsync(long id)
        {
            return await _dbSet
                .Include(v => v.Specials)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venue?> GetWithAllDataAsync(long id)
        {
            return await _dbSet
                .Include(v => v.VenueType)
                .Include(v => v.BusinessHours)
                .Include(v => v.Specials)
                    .ThenInclude(s => s.Tags)
                        .ThenInclude(tsl => tsl.Tag)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
