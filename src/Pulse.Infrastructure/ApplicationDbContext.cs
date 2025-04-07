namespace Pulse.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    using Pulse.Core.Enums;
    using Pulse.Core.Models.Entities;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }

        public DbSet<Special> Specials { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.HasPostgresEnum<SpecialTypes>();

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(v => v.Description)
                    .HasMaxLength(1000);
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
                entity.Property(v => v.PhoneNumber)
                    .HasMaxLength(20);
                entity.Property(v => v.Website)
                    .HasMaxLength(255);
                entity.Property(v => v.Email)
                    .HasMaxLength(255);
            });

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
                entity.Property(s => s.RecurringSchedule)
                    .HasMaxLength(50);
                entity.HasOne(s => s.Venue)
                      .WithMany(v => v.Specials)
                      .HasForeignKey(s => s.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
