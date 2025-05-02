namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class ApplicationPermissionConfiguration : IEntityTypeConfiguration<ApplicationPermission>
    {
        public void Configure(EntityTypeBuilder<ApplicationPermission> builder)
        {
            builder.ToTable("application_permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the permission.");

            builder.Property(p => p.Value)
                .HasColumnName("value")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The unique programmatic name of the permission, e.g., 'specials:edit'.");

            builder.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired()
                .HasComment("A description of the permission's purpose, e.g., 'Allows editing of specials.'.");

            builder.Property(p => p.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the permission is active.");

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the permission was created.");

            builder.Property(p => p.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .HasComment("The ID of the user who created the permission, if applicable.");

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .HasComment("The timestamp when the permission was last updated, if applicable.");

            builder.Property(p => p.UpdatedByUserId)
                .HasColumnName("updated_by_user_id")
                .HasComment("The ID of the user who last updated the permission, if applicable.");

            builder.HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UpdatedByUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.Value)
                .IsUnique()
                .HasDatabaseName("ix_application_permissions_value");
        }
    }
}
