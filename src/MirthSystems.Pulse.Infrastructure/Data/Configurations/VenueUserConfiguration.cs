namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class VenueUserConfiguration : IEntityTypeConfiguration<VenueUser>
    {
        public void Configure(EntityTypeBuilder<VenueUser> builder)
        {
            builder.ToTable("venue_users");

            builder.HasKey(vu => vu.Id);

            builder.Property(vu => vu.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the venue-user association.");

            builder.Property(vu => vu.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationUser.");

            builder.Property(vu => vu.VenueId)
                .HasColumnName("venue_id")
                .IsRequired()
                .HasComment("The foreign key to the Venue.");

            builder.Property(vu => vu.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the association was created.");

            builder.Property(vu => vu.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .HasComment("The ID of the user who created the association, if applicable.");

            builder.Property(vu => vu.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the association is active.");

            builder.HasOne(vu => vu.User)
                .WithMany(u => u.VenueUsers)
                .HasForeignKey(vu => vu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vu => vu.Venue)
                .WithMany(v => v.VenueUsers)
                .HasForeignKey(vu => vu.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vu => vu.CreatedByUser)
                .WithMany()
                .HasForeignKey(vu => vu.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(vu => new { vu.UserId, vu.VenueId })
                .IsUnique()
                .HasDatabaseName("ix_venue_users_user_id_venue_id");
        }
    }
}
