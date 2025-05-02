namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class VenueRolePermissionConfiguration : IEntityTypeConfiguration<VenueRolePermission>
    {
        public void Configure(EntityTypeBuilder<VenueRolePermission> builder)
        {
            builder.ToTable("venue_role_permissions");

            builder.HasKey(rp => rp.Id);

            builder.Property(rp => rp.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the role-permission association.");

            builder.Property(rp => rp.VenueRoleId)
                .HasColumnName("venue_role_id")
                .IsRequired()
                .HasComment("The foreign key to the VenueRole.");

            builder.Property(rp => rp.PermissionId)
                .HasColumnName("permission_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationPermission.");

            builder.Property(rp => rp.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the permission was assigned to the venue role.");

            builder.Property(rp => rp.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .HasComment("The ID of the user who assigned the permission, if applicable.");

            builder.HasOne(rp => rp.VenueRole)
                .WithMany(r => r.Permissions)
                .HasForeignKey(rp => rp.VenueRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.VenueRoles)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.CreatedByUser)
                .WithMany()
                .HasForeignKey(rp => rp.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(rp => new { rp.VenueRoleId, rp.PermissionId })
                .IsUnique()
                .HasDatabaseName("ix_venue_role_permissions_venue_role_id_permission_id");
        }
    }
}
