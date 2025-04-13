namespace Pulse.DatabaseMigrationService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Pulse.Core.Models.Entities;
    using Pulse.Infrastructure;

    public static class DataSeeder
    {
        public static async Task RunAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            await SeedVenueTypesAsync(dbContext, cancellationToken);
        }
        private static async Task SeedVenueTypesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            if (!await dbContext.VenueTypes.AnyAsync(cancellationToken))
            {
                var defaultVenueTypes = new List<VenueType>
                {
                    new VenueType { Name = "Unknown" },
                    new VenueType { Name = "Restaurant" },
                    new VenueType { Name = "Bar" },
                    new VenueType { Name = "Cafe" },
                    new VenueType { Name = "Club" }
                };

                dbContext.VenueTypes.AddRange(defaultVenueTypes);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
