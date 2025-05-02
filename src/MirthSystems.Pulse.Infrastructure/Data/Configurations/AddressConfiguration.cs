namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the address, auto-incremented by the database.");

            builder.Property(a => a.AddressLine1)
                .HasColumnName("address_line1")
                .HasMaxLength(255)
                .IsRequired()
                .HasComment("The primary address information, e.g., '123 Main St'.");

            builder.Property(a => a.AddressLine2)
                .HasColumnName("address_line2")
                .HasMaxLength(255)
                .HasComment("Optional additional address details, e.g., 'Suite 200'.");

            builder.Property(a => a.Locality)
                .HasColumnName("locality")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The city or town, e.g., 'Springfield'.");

            builder.Property(a => a.Region)
                .HasColumnName("region")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The administrative region, e.g., 'Illinois'.");

            builder.Property(a => a.Postcode)
                .HasColumnName("postcode")
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("The postal code, e.g., '62701'.");

            builder.Property(a => a.Country)
                .HasColumnName("country")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("The country, e.g., 'United States'.");

            builder.Property(a => a.Location)
                .HasColumnName("location")
                .HasColumnType("geometry(Point, 4326)")
                .HasComment("The geographic coordinates as a Point (longitude, latitude) with SRID 4326.");
        }
    }
}
