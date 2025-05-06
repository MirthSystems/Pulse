namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;

    using NetTopologySuite.Geometries;

    public class VenueRepository : Repository<Venue>, IVenueRepository
    {
        public VenueRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<Venue> GetAll()
        {
            return _dbSet.Where(v => !v.IsDeleted);
        }

        public async Task<Venue?> GetVenueWithDetailsAsync(long id)
        {
            return await _context.Venues
                .Include(v => v.Address)
                .Include(v => v.BusinessHours)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        public async Task<(List<Venue> venues, int totalCount)> GetPagedVenuesAsync(int page, int pageSize)
        {
            var query = _context.Venues
                .Include(v => v.Address)
                .Where(v => !v.IsDeleted)
                .OrderByDescending(v => v.CreatedAt);

            var totalCount = await query.CountAsync();

            var venues = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (venues, totalCount);
        }

        public async Task<List<Venue>> FindVenuesNearLocationAsync(Point location, double distanceInMeters)
        {
            return await _context.Venues
                .Include(v => v.Address)
                .Where(v => !v.IsDeleted &&
                           v.Address.Location.Distance(location) <= distanceInMeters)
                .OrderBy(v => v.Address.Location.Distance(location))
                .ToListAsync();
        }
    }
}
