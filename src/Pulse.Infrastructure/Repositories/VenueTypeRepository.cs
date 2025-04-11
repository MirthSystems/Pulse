namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class VenueTypeRepository : Repository<VenueType, int>, IVenueTypeRepository
    {
        private static readonly Func<ApplicationDbContext, string, Task<VenueType?>> _getByNameQuery =
            EF.CompileAsyncQuery((ApplicationDbContext context, string name) =>
                context.VenueTypes
                    .AsNoTracking()
                    .FirstOrDefault(vt => vt.Name.ToLower() == name.ToLower()));

        public VenueTypeRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<VenueType?> GetByNameAsync(string name)
        {
            return await _getByNameQuery(_context, name.ToLower());
        }

        public async Task<VenueType?> GetWithVenuesAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(vt => vt.Venues)
                .FirstOrDefaultAsync(vt => vt.Id == id);
        }
    }
}
