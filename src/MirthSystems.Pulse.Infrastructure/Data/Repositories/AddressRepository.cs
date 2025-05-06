namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;

    using NetTopologySuite.Geometries;

    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<StandardizedAddress> StandardizeAddressAsync(string addressString)
        {
            var result = await _context.Database.SqlQueryRaw<StandardizedAddress>(
                @"SELECT building, house_num, predir, qual, pretype, name, suftype, sufdir, ruralroute, extra, city, state, country, postcode, box, unit   
                     FROM standardize_address('us_lex', 'us_gaz', 'us_rules', @addressString)",
                new SqlParameter("@addressString", addressString))
                .FirstOrDefaultAsync();

            return result ?? new StandardizedAddress();
        }

        public async Task<Address?> FindNearestAddressAsync(Point location, double maxDistanceInMeters)
        {
            return await _context.Addresses
                .OrderBy(a => a.Location.Distance(location))
                .FirstOrDefaultAsync(a => a.Location.Distance(location) <= maxDistanceInMeters);
        }
    }
}
