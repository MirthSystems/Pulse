namespace Pulse.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    using Pulse.Core.Enums;
    using Pulse.Core.Models.Entities;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueType> VenueTypes { get; set; }
        public DbSet<Special> Specials { get; set; }
        public DbSet<OperatingSchedule> BusinessHours { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagToSpecialLink> TagToSpecialLinks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("address_standardizer");
            modelBuilder.HasPostgresExtension("address_standardizer_data_us");
            modelBuilder.HasPostgresExtension("fuzzystrmatch");
            modelBuilder.HasPostgresExtension("plpgsql");
            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.HasPostgresExtension("postgis_raster");
            modelBuilder.HasPostgresExtension("postgis_sfcgal");
            modelBuilder.HasPostgresExtension("postgis_tiger_geocoder");
            modelBuilder.HasPostgresExtension("postgis_topology");

            modelBuilder.HasPostgresEnum<SpecialTypes>();
            modelBuilder.HasPostgresEnum<DayOfWeek>();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(EntityBase.CreatedAt))
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(EntityBase.CreatedByUserId))
                        .HasMaxLength(128);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(EntityBase.UpdatedAt));

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(EntityBase.UpdatedByUserId))
                        .HasMaxLength(128);

                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(EntityBase.CreatedAt));

                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(EntityBase.CreatedByUserId));
                }
            }

            #region Venue Configuration
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.VenueTypeId)
                    .IsRequired();

                entity.Property(v => v.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(v => v.Description)
                    .HasMaxLength(1000);

                entity.Property(v => v.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(v => v.Website)
                    .HasMaxLength(255);

                entity.Property(v => v.Email)
                    .HasMaxLength(255);

                entity.Property(v => v.ImageLink)
                    .HasMaxLength(512);

                entity.Property(v => v.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(v => v.AddressLine2)
                    .HasMaxLength(255);

                entity.Property(v => v.AddressLine3)
                    .HasMaxLength(255);

                entity.Property(v => v.AddressLine4)
                    .HasMaxLength(255);

                entity.Property(v => v.Locality)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.Region)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.Postcode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(v => v.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.Location)
                    .HasColumnType("geography");

                entity.HasOne(v => v.VenueType)
                    .WithMany(vt => vt.Venues)
                    .HasForeignKey(v => v.VenueTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(v => v.Name);
                entity.HasIndex(v => v.VenueTypeId);
                entity.HasIndex(v => v.Location)
                    .HasMethod("GIST");
            });
            #endregion

            #region VenueType Configuration
            modelBuilder.Entity<VenueType>(entity =>
            {
                entity.HasKey(vt => vt.Id);

                entity.Property(vt => vt.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(vt => vt.Description)
                    .HasMaxLength(255);

                entity.HasIndex(vt => vt.Name)
                    .IsUnique();
            });
            #endregion

            #region OperatingSchedule Configuration
            modelBuilder.Entity<OperatingSchedule>(entity =>
            {
                entity.HasKey(os => os.Id);

                entity.Property(os => os.VenueId)
                    .IsRequired();

                entity.Property(os => os.DayOfWeek)
                    .IsRequired();

                entity.Property(os => os.TimeOfOpen)
                    .IsRequired();

                entity.Property(os => os.TimeOfClose)
                    .IsRequired();

                entity.Property(os => os.IsClosed)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasIndex(os => new { os.VenueId, os.DayOfWeek })
                    .IsUnique();

                entity.HasOne(os => os.Venue)
                    .WithMany(v => v.BusinessHours)
                    .HasForeignKey(os => os.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Special Configuration
            modelBuilder.Entity<Special>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.VenueId)
                    .IsRequired();

                entity.Property(s => s.Content)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(s => s.Type)
                    .IsRequired();

                entity.Property(s => s.StartDate)
                    .IsRequired();

                entity.Property(s => s.StartTime)
                    .IsRequired();

                entity.Property(s => s.EndTime);

                entity.Property(s => s.ExpirationDate);

                entity.Property(s => s.IsRecurring)
                    .IsRequired();

                entity.Property(s => s.RecurringPeriod)
                    .HasColumnType("interval");

                entity.Property(s => s.ActiveDaysOfWeek);

                entity.HasOne(s => s.Venue)
                    .WithMany(v => v.Specials)
                    .HasForeignKey(s => s.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(s => s.VenueId);
                entity.HasIndex(s => s.Type);
                entity.HasIndex(s => s.StartDate);
                entity.HasIndex(s => s.ExpirationDate);
                entity.HasIndex(s => new { s.StartDate, s.StartTime });
            });
            #endregion

            #region Tag Configuration
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.UsageCount)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasIndex(t => t.Name)
                    .IsUnique();

                entity.HasIndex(t => t.UsageCount);
            });
            #endregion

            #region TagToSpecialLink Configuration
            modelBuilder.Entity<TagToSpecialLink>(entity =>
            {
                entity.HasKey(ts => new { ts.TagId, ts.SpecialId });

                entity.HasOne(ts => ts.Tag)
                    .WithMany(t => t.Specials)
                    .HasForeignKey(ts => ts.TagId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ts => ts.Special)
                    .WithMany(s => s.Tags)
                    .HasForeignKey(ts => ts.SpecialId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(ts => ts.TagId);
                entity.HasIndex(ts => ts.SpecialId);
            });
            #endregion
        }
    }
}