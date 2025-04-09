namespace Pulse.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    using Pulse.Core.Enums;
    using Pulse.Core.Models.Entities;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; } = null!;
        public DbSet<VenueType> VenueTypes { get; set; } = null!;
        public DbSet<Special> Specials { get; set; } = null!;
        public DbSet<OperatingSchedule> BusinessHours { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<TagSpecial> TagSpecials { get; set; } = null!;
        public DbSet<ApplicationUser> Users { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Vibe> Vibes { get; set; } = null!;
        public DbSet<PostVibe> PostVibes { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.HasPostgresEnum<SpecialTypes>();
            modelBuilder.HasPostgresEnum<DayOfWeek>();

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

                entity.Property(s => s.RecurringSchedule)
                    .HasMaxLength(50);

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

                entity.Property(t => t.CreatedAt)
                    .IsRequired();

                entity.HasIndex(t => t.Name)
                    .IsUnique();

                entity.HasIndex(t => t.UsageCount);
            });
            #endregion

            #region TagSpecial Configuration
            modelBuilder.Entity<TagSpecial>(entity =>
            {
                entity.HasKey(ts => new { ts.TagId, ts.SpecialId });

                entity.ToTable("tags_specials");

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

            #region ApplicationUser Configuration
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.ToTable("users");

                entity.Property(u => u.ProviderId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(u => u.CreatedAt)
                    .IsRequired();

                entity.Property(u => u.DefaultSearchLocationString)
                    .HasMaxLength(255);

                entity.Property(u => u.DefaultSearchLocation)
                    .HasColumnType("geography");

                entity.Property(u => u.DefaultSearchRadius)
                    .IsRequired()
                    .HasDefaultValue(5.0);

                entity.Property(u => u.OptedInToLocationServices)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasIndex(u => u.ProviderId)
                    .IsUnique();

                entity.HasIndex(u => u.DefaultSearchLocation)
                    .HasMethod("GIST");

                entity.HasIndex(u => u.CreatedAt);
                entity.HasIndex(u => u.LastLoginAt);
            });
            #endregion

            #region Post Configuration
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserId)
                    .IsRequired();

                entity.Property(p => p.VenueId)
                    .IsRequired();

                entity.Property(p => p.TextContent)
                    .HasMaxLength(280);

                entity.Property(p => p.ImageUrl)
                    .HasMaxLength(512);

                entity.Property(p => p.VideoUrl)
                    .HasMaxLength(512);

                entity.Property(p => p.CreatedAt)
                    .IsRequired();

                entity.Property(p => p.ExpiresAt)
                    .IsRequired();

                entity.Property(p => p.IsExpired)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Venue)
                    .WithMany(v => v.Posts)
                    .HasForeignKey(p => p.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.UserId);
                entity.HasIndex(p => p.VenueId);
                entity.HasIndex(p => p.CreatedAt);
                entity.HasIndex(p => p.ExpiresAt);
                entity.HasIndex(p => p.IsExpired);

                entity.HasIndex(p => new { p.VenueId, p.IsExpired });
                entity.HasIndex(p => new { p.VenueId, p.ExpiresAt });
            });
            #endregion

            #region Vibe Configuration
            modelBuilder.Entity<Vibe>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.UsageCount)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(v => v.CreatedAt)
                    .IsRequired();

                entity.HasIndex(v => v.Name)
                    .IsUnique();

                entity.HasIndex(v => v.UsageCount);
            });
            #endregion

            #region PostVibe Configuration
            modelBuilder.Entity<PostVibe>(entity =>
            {
                entity.HasKey(pv => new { pv.PostId, pv.VibeId });

                entity.ToTable("posts_vibes");

                entity.HasOne(pv => pv.Post)
                    .WithMany(p => p.Vibes)
                    .HasForeignKey(pv => pv.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pv => pv.Vibe)
                    .WithMany(v => v.Posts)
                    .HasForeignKey(pv => pv.VibeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(pv => pv.PostId);
                entity.HasIndex(pv => pv.VibeId);
            });
            #endregion
        }
    }
}
