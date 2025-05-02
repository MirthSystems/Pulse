namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class ApplicationUserPermissionConfiguration : IEntityTypeConfiguration<ApplicationUserPermission>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserPermission> builder)
        {
            builder.ToTable("application_user_permissions");

            builder.HasKey(up => up.Id);

            builder.Property(up => up.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the user-permission association.");

            builder.Property(up => up.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationUser.");

            builder.Property(up => up.PermissionId)
                .HasColumnName("permission_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationPermission.");

            builder.Property(up => up.AssignedAt)
                .HasColumnName("assigned_at")
                .IsRequired()
                .HasComment("The timestamp when the permission was assigned to the user.");

            builder.Property(up => up.AssignedByUserId)
                .HasColumnName("assigned_by_user_id")
                .HasComment("The ID of the user who assigned the permission, if applicable.");

            builder.Property(up => up.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the permission assignment is active.");

            builder.Property(up => up.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .HasComment("Indicates if the permission assignment has been soft-deleted.");

            builder.Property(up => up.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the permission assignment was deleted, if applicable.");

            builder.Property(up => up.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasComment("The ID of the user who deleted the permission assignment, if applicable.");

            builder.HasOne(up => up.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.Permission)
                .WithMany(p => p.Users)
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.AssignedByUser)
                .WithMany()
                .HasForeignKey(up => up.AssignedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(up => up.DeletedByUser)
                .WithMany()
                .HasForeignKey(up => up.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(up => new { up.UserId, up.PermissionId })
                .IsUnique()
                .HasDatabaseName("ix_application_user_permissions_user_id_permission_id");
        }
    }
}
