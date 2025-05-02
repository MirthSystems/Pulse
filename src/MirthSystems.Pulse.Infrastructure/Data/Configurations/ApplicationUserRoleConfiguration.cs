namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("application_user_roles");

            builder.HasKey(ur => ur.Id);

            builder.Property(ur => ur.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the user-role association.");

            builder.Property(ur => ur.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationUser.");

            builder.Property(ur => ur.RoleId)
                .HasColumnName("role_id")
                .IsRequired()
                .HasComment("The foreign key to the ApplicationRole.");

            builder.Property(ur => ur.AssignedAt)
                .HasColumnName("assigned_at")
                .IsRequired()
                .HasComment("The timestamp when the role was assigned to the user.");

            builder.Property(ur => ur.AssignedByUserId)
                .HasColumnName("assigned_by_user_id")
                .HasComment("The ID of the user who assigned the role, if applicable.");

            builder.Property(ur => ur.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the role assignment is active.");

            builder.Property(ur => ur.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .HasComment("Indicates if the role assignment has been soft-deleted.");

            builder.Property(ur => ur.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the role assignment was deleted, if applicable.");

            builder.Property(ur => ur.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasComment("The ID of the user who deleted the role assignment, if applicable.");

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.AssignedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.AssignedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ur => ur.DeletedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique()
                .HasDatabaseName("ix_application_user_roles_user_id_role_id");
        }
    }
}
