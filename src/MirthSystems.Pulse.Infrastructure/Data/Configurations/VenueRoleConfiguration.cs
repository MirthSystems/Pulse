namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class VenueRoleConfiguration : IEntityTypeConfiguration<VenueRole>
    {
        public void Configure(EntityTypeBuilder<VenueRole> builder)
        {
            builder.ToTable("venue_roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the venue role.");

            builder.Property(r => r.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The human-readable name of the role, e.g., 'Venue Owner'.");

            builder.Property(r => r.Value)
                .HasColumnName("value")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The programmatic name of the role, e.g., 'Venue.Owner'.");

            builder.Property(r => r.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired()
                .HasComment("A description of the role's purpose.");

            builder.Property(r => r.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .HasComment("Indicates if the role is active for new assignments.");

            builder.Property(r => r.DisplayOrder)
                .HasColumnName("display_order")
                .HasDefaultValue(0)
                .HasComment("The order for displaying the role in lists.");

            builder.HasIndex(r => r.Value)
                .IsUnique()
                .HasDatabaseName("ix_venue_roles_value");
        }
    }
}
