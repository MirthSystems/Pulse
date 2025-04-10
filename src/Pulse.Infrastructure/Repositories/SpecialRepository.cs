namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Enums;
    using Pulse.Core.Models.Entities;
    using Pulse.Core.Utilities;

    public class SpecialRepository : Repository<Special, long>, ISpecialRepository
    {
        public SpecialRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<IEnumerable<Special>> GetActiveForVenueAsync(long venueId)
        {
            // Get all specials for the venue
            var specials = await _dbSet
                .Where(s => s.VenueId == venueId)
                .ToListAsync();

            // Get current instant for filtering
            var now = _clock.GetCurrentInstant();

            // Filter in memory using the helper utility
            return specials.Where(s => SpecialHelper.IsActive(s, now));
        }

        public async Task<IEnumerable<Special>> GetWithTagsForVenueAsync(long venueId)
        {
            return await _dbSet
                .Include(s => s.Tags)
                    .ThenInclude(tsl => tsl.Tag)
                .Where(s => s.VenueId == venueId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Special>> GetActiveByTypeAsync(SpecialTypes type)
        {
            // Get all specials of this type
            var specials = await _dbSet
                .Where(s => s.Type == type)
                .ToListAsync();

            // Get current instant for filtering
            var now = _clock.GetCurrentInstant();

            // Filter in memory using the helper utility
            return specials.Where(s => SpecialHelper.IsActive(s, now));
        }

        public async Task<Special?> GetWithTagsAsync(long id)
        {
            return await _dbSet
                .Include(s => s.Tags)
                    .ThenInclude(tsl => tsl.Tag)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Special?> GetWithVenueAsync(long id)
        {
            return await _dbSet
                .Include(s => s.Venue)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Special?> GetWithAllDataAsync(long id)
        {
            return await _dbSet
                .Include(s => s.Venue)
                .Include(s => s.Tags)
                    .ThenInclude(tsl => tsl.Tag)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
