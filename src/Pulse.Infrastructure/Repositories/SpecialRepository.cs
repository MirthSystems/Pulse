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
        private static readonly Func<ApplicationDbContext, long, Task<List<Special>>> _getSpecialsForVenueQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long venueId) =>
                context.Specials
                    .AsNoTracking()
                    .Where(s => s.VenueId == venueId)
                    .ToList());

        private static readonly Func<ApplicationDbContext, long, Task<Special?>> _getWithAllDataQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, long id) =>
                context.Specials
                    .Include(s => s.Venue)
                    .Include(s => s.Tags)
                        .ThenInclude(tsl => tsl.Tag)
                    .FirstOrDefault(s => s.Id == id));

        public SpecialRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<IEnumerable<Special>> GetActiveForVenueAsync(long venueId)
        {
            var specials = await _getSpecialsForVenueQuery(_context, venueId);

            var now = _clock.GetCurrentInstant();

            return SpecialHelper.GetActiveSpecials(specials, now).ToList();
        }

        public async Task<IEnumerable<Special>> GetWithTagsForVenueAsync(long venueId)
        {
            return await _dbSet
                .AsSplitQuery()
                .AsNoTracking()
                .Include(s => s.Tags)
                    .ThenInclude(tsl => tsl.Tag)
                .Where(s => s.VenueId == venueId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Special>> GetActiveByTypeAsync(SpecialTypes type)
        {
            var specials = await _dbSet
                .AsNoTracking()
                .Where(s => s.Type == type)
                .ToListAsync();

            var now = _clock.GetCurrentInstant();

            return SpecialHelper.GetActiveSpecials(specials, now).ToList();
        }

        public async Task<Special?> GetWithTagsAsync(long id)
        {
            return await _dbSet
                .AsSplitQuery()
                .AsNoTracking()
                .Include(s => s.Tags)
                    .ThenInclude(tsl => tsl.Tag)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Special?> GetWithVenueAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(s => s.Venue)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Special?> GetWithAllDataAsync(long id)
        {
            return await _getWithAllDataQuery(_context, id);
        }
    }
}
