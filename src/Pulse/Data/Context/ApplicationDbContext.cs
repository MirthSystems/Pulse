namespace Pulse.Data.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Pulse.Data.Entities;

    using DayOfWeek = Pulse.Data.Entities.DayOfWeek;

    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<BusinessHours> BusinessHours => Set<BusinessHours>();
        public DbSet<DayOfWeek> DayOfWeeks => Set<DayOfWeek>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<VenueCategory> VenueCategories => Set<VenueCategory>();
        public DbSet<Special> Specials => Set<Special>(); 
        public DbSet<SpecialCategory> SpecialCategories => Set<SpecialCategory>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("address_standardizer");
            modelBuilder.HasPostgresExtension("address_standardizer_data_us");
            modelBuilder.HasPostgresExtension("fuzzystrmatch");
            modelBuilder.HasPostgresExtension("plpgsql");
            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.HasPostgresExtension("postgis_raster");
            modelBuilder.HasPostgresExtension("postgis_sfcgal");
            modelBuilder.HasPostgresExtension("postgis_tiger_geocoder");
            modelBuilder.HasPostgresExtension("postgis_topology");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
