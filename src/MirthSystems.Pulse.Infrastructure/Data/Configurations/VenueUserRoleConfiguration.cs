namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class VenueUserRoleConfiguration : IEntityTypeConfiguration<VenueUserRole>
    {
        public void Configure(EntityTypeBuilder<VenueUserRole> builder)
        {
            builder.ToTable("venue_user_roles");

            builder.HasKey(vur => vur.Id);

            builder.Property(vur => vur.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the venue-user-role association.");

            builder.Property(vur => vur.VenueUserId)
                .HasColumnName("venue_user_id")
                .IsRequired()
                .HasComment("The foreign key to the VenueUser.");

            builder.Property(vur => vur.VenueRoleId)
                .HasColumnName("venue_role_id")
                .IsRequired()
                .HasComment("The foreign key to the VenueRole.");

            builder.Property(vur => vur.AssignedAt)
                .HasColumnName("assigned_at")
                .IsRequired()
                .HasComment("The timestamp when the role was assigned.");

            builder.Property(vur => vur.AssignedByUserId)
                .HasColumnName("assigned_by_user_id")
                .HasComment("The ID of the user who assigned the role, if applicable.");

            builder.Property(vur => vur.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the role assignment is active.");

            builder.Property(vur => vur.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .HasComment("Indicates if the role assignment is soft-deleted.");

            builder.Property(vur => vur.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the role was soft-deleted, if applicable.");

            builder.Property(vur => vur.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasComment("The ID of the user who deleted the role, if applicable.");

            builder.HasOne(vur => vur.VenueUser)
                .WithMany(vu => vu.VenueRoles)
                .HasForeignKey(vur => vur.VenueUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vur => vur.VenueRole)
                .WithMany(vr => vr.VenueUsers)
                .HasForeignKey(vur => vur.VenueRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vur => vur.AssignedByUser)
                .WithMany()
                .HasForeignKey(vur => vur.AssignedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vur => vur.DeletedByUser)
                .WithMany()
                .HasForeignKey(vur => vur.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(vur => new { vur.VenueUserId, vur.VenueRoleId })
                .IsUnique()
                .HasDatabaseName("ix_venue_user_roles_venue_user_id_venue_role_id");
        }
    }
}
