namespace Pulse.DatabaseMigrationService
{
    using Microsoft.EntityFrameworkCore;

    using Pulse.Core.Models.Entities;
    using Pulse.Infrastructure;

    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(ApplicationDbContext dbContext, ILogger<DatabaseSeeder> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task SeedAsync()
        {
            this._logger.LogInformation("Beginning database seeding...");

            await SeedVenuePermissionsAsync();

            await _dbContext.SaveChangesAsync();
            this._logger.LogInformation("Database seeding completed successfully.");
        }

        private async Task SeedVenuePermissionsAsync()
        {
            if (!await this._dbContext.VenuePermissions.AnyAsync())
            {
                this._logger.LogInformation("Seeding venue permissions...");

                var permissions = new List<VenuePermission>
                {
                    new VenuePermission
                    {
                        Name = "manage:venue",
                        Description = "Manage venue details including address, contact information, and hours"
                    },
                    new VenuePermission
                    {
                        Name = "manage:specials",
                        Description = "Create, edit, and delete specials and promotions for the venue"
                    },
                    new VenuePermission
                    {
                        Name = "respond:posts",
                        Description = "Respond to customer posts about the venue"
                    },
                    new VenuePermission
                    {
                        Name = "invite:users",
                        Description = "Invite other users to manage the venue"
                    },
                    new VenuePermission
                    {
                        Name = "manage:users",
                        Description = "Manage user permissions for the venue"
                    }
                };

                await this._dbContext.VenuePermissions.AddRangeAsync(permissions);
                await this._dbContext.SaveChangesAsync();
                this._logger.LogInformation("Venue permissions seeded successfully.");
            }
            else
            {
                this._logger.LogInformation("Venue permissions already exist - skipping seeding.");
            }
        }
    }
}
