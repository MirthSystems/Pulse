namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("application_roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the application role.");

            builder.Property(r => r.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The human-readable name of the role, e.g., 'System Administrator'.");

            builder.Property(r => r.Value)
                .HasColumnName("value")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The programmatic name of the role, e.g., 'System.Administrator'.");

            builder.Property(r => r.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired()
                .HasComment("A description of the role's purpose.");

            builder.Property(r => r.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the role is active.");

            builder.Property(r => r.DisplayOrder)
                .HasColumnName("display_order")
                .HasDefaultValue(0)
                .HasComment("The order for displaying the role in lists.");

            builder.Property(r => r.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the role was created.");

            builder.Property(r => r.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .HasComment("The ID of the user who created the role, if applicable.");

            builder.Property(r => r.UpdatedAt)
                .HasColumnName("updated_at")
                .HasComment("The timestamp when the role was last updated, if applicable.");

            builder.Property(r => r.UpdatedByUserId)
                .HasColumnName("updated_by_user_id")
                .HasComment("The ID of the user who last updated the role, if applicable.");

            builder.Property(r => r.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .HasComment("Indicates if the role has been soft-deleted.");

            builder.Property(r => r.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the role was deleted, if applicable.");

            builder.Property(r => r.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasComment("The ID of the user who deleted the role, if applicable.");

            builder.HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.UpdatedByUser)
                .WithMany()
                .HasForeignKey(r => r.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.DeletedByUser)
                .WithMany()
                .HasForeignKey(r => r.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.Value)
                .IsUnique()
                .HasDatabaseName("ix_application_roles_value");
        }
    }
}
