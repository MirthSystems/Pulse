namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class ApplicationRolePermissionConfiguration : IEntityTypeConfiguration<ApplicationRolePermission>
    {
        public void Configure(EntityTypeBuilder<ApplicationRolePermission> builder)
        {
            builder.ToTable("application_role_permissions");

            builder.HasKey(rp => rp.Id);

            builder.Property(rp => rp.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the role-permission association.");

            builder.Property(rp => rp.RoleId)
                .HasColumnName("role_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationRole.");

            builder.Property(rp => rp.PermissionId)
                .HasColumnName("permission_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationPermission.");

            builder.Property(rp => rp.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the permission was assigned to the role.");

            builder.Property(rp => rp.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .HasComment("The ID of the user who assigned the permission, if applicable.");

            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.Roles)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.CreatedByUser)
                .WithMany()
                .HasForeignKey(rp => rp.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .IsUnique()
                .HasDatabaseName("ix_application_role_permissions_role_id_permission_id");
        }
    }
}
