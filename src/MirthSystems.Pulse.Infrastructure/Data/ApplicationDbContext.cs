namespace MirthSystems.Pulse.Infrastructure.Data
{
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;

    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Enums;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<OperatingSchedule> OperatingSchedules { get; set; }
        public DbSet<Special> Specials { get; set; }   

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        /// <remarks>
        /// <para>This method is called when the model for a derived context has been initialized, but before the model has been locked down.</para>
        /// <para>It configures the following aspects of the database:</para>
        /// <para>- PostgreSQL extensions including PostGIS for spatial data and pg_cron for scheduling</para>
        /// <para>- Custom enum types for PostgreSQL</para>
        /// <para>- Entity configurations from dedicated configuration classes</para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure PostgreSQL extensions for spatial data and address handling
            modelBuilder.HasPostgresExtension("address_standardizer");
            modelBuilder.HasPostgresExtension("address_standardizer_data_us");
            modelBuilder.HasPostgresExtension("fuzzystrmatch");
            modelBuilder.HasPostgresExtension("plpgsql");
            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.HasPostgresExtension("postgis_raster");
            modelBuilder.HasPostgresExtension("postgis_sfcgal");
            modelBuilder.HasPostgresExtension("postgis_tiger_geocoder");
            modelBuilder.HasPostgresExtension("postgis_topology");

            // Configure PostgreSQL enum types
            modelBuilder.HasPostgresEnum<DayOfWeek>();
            modelBuilder.HasPostgresEnum<SpecialTypes>();

            // Apply all configurations from assembly (using dedicated configuration classes)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }        
    }
}
