namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using NetTopologySuite.Algorithm;
    using NetTopologySuite.Algorithm.Distance;
    using NetTopologySuite.Geometries;

    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models;
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

        public async Task<IEnumerable<VenueWithDistance>> FindVenuesNearbyAsync(Point location, double radiusMiles)
        {
            location = LocationHelper.EnsureSrid(location);
            double radiusMeters = LocationHelper.MilesToMeters(radiusMiles);

            var venues = await _dbSet
                .AsNoTracking()
                .Where(v => v.Location != null && v.Location.IsWithinDistance(location, radiusMeters))
                .Select(v => new VenueWithDistance
                {
                    Venue = v,
                    DistanceMiles = v.Location!.Distance(location) / LocationHelper.MetersPerMile,
                    SearchPoint = location
                })
                .OrderBy(v => v.DistanceMiles)
                .ToListAsync();

            return venues;
        }

        public async Task<IEnumerable<VenueWithDistance>> FindVenuesWithActiveSpecialsNearbyAsync(
            Point location,
            double radiusMiles,
            Instant currentInstant)
        {
            location = LocationHelper.EnsureSrid(location);
            double radiusMeters = LocationHelper.MilesToMeters(radiusMiles);

            var venues = await _dbSet
                .AsNoTracking()
                .Where(v => v.Location != null && v.Location.IsWithinDistance(location, radiusMeters))
                .Select(v => new VenueWithDistance
                {
                    Venue = v,
                    DistanceMiles = v.Location!.Distance(location) / LocationHelper.MetersPerMile,
                    SearchPoint = location
                })
                .OrderBy(v => v.DistanceMiles)
                .ToListAsync();

            var venueIds = venues.Select(v => v.Venue.Id).ToList();
            var venuesWithSpecials = await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Where(v => venueIds.Contains(v.Id))
                .Include(v => v.Specials)
                .ToListAsync();

            var venuesLookup = venuesWithSpecials.ToDictionary(v => v.Id);

            var result = new List<VenueWithDistance>();
            foreach (var venueWithDistance in venues)
            {
                if (venuesLookup.TryGetValue(venueWithDistance.Venue.Id, out var venueWithSpecials) &&
                    venueWithSpecials.Specials.Any(s => SpecialHelper.IsActive(s, currentInstant)))
                {
                    venueWithDistance.Venue = venueWithSpecials;
                    result.Add(venueWithDistance);
                }
            }

            return result;
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
    }
}
