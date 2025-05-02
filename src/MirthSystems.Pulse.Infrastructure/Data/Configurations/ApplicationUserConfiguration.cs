namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the user within the application.");

            builder.Property(u => u.UserObjectId)
                .HasColumnName("user_object_id")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The Auth0 user ID, e.g., 'auth0|12345abcde'.");

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the user record was created, stored as UTC.");

            builder.Property(u => u.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the user account is active.");

            builder.HasIndex(u => u.UserObjectId)
                .IsUnique()
                .HasDatabaseName("ix_application_users_user_object_id");
        }
    }
}
