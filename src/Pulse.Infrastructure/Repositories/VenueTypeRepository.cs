namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;

    public class VenueTypeRepository : Repository<VenueType, int>, IVenueTypeRepository
    {
        public VenueTypeRepository(ApplicationDbContext context, IClock clock)
            : base(context, clock)
        {
        }

        public async Task<VenueType?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(vt => vt.Name.ToLower() == name.ToLower());
        }

        public async Task<VenueType?> GetWithVenuesAsync(int id)
        {
            return await _dbSet
                .Include(vt => vt.Venues)
                .FirstOrDefaultAsync(vt => vt.Id == id);
        }
    }
}
