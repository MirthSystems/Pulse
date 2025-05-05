namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Entities;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");

            builder.HasKey(a => a.Id)
                .HasName("pk_addresses");

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the address. This is the primary key for the address entity in the database.");

            builder.Property(a => a.StreetAddress)
                .IsRequired()
                .HasColumnName("street_address")
                .HasMaxLength(255)
                .HasComment("This required field captures the primary address information, typically including the street number and name. Examples include: '123 Main St' (USA), '10 Downing Street' (UK), '35 Rue du Faubourg Saint-Honoré' (France).");

            builder.Property(a => a.SecondaryAddress)
                .HasColumnName("secondary_address")
                .HasMaxLength(255)
                .HasComment("This optional field captures additional address details like suite or unit numbers. Examples include: 'Suite 200' (USA), 'Flat 3' (UK), 'Apartment 12B' (General).");

            builder.Property(a => a.Locality)
                .IsRequired()
                .HasColumnName("locality")
                .HasMaxLength(100)
                .HasComment("The city, town, or locality where the address is located. Examples include: 'Springfield' (USA), 'Toronto' (Canada), 'Manchester' (UK).");

            builder.Property(a => a.Region)
                .IsRequired()
                .HasColumnName("region")
                .HasMaxLength(100)
                .HasComment("The administrative region where the address is located. This required field can represent a state, province, territory, or other regional division depending on the country. Examples include: 'Illinois' (USA), 'Ontario' (Canada), 'Queensland' (Australia), 'Bavaria' (Germany).");

            builder.Property(a => a.Postcode)
                .IsRequired()
                .HasColumnName("postcode")
                .HasMaxLength(20)
                .HasComment("The postal or ZIP code of the address's location. This required field supports various international formats. Examples include: '62701' (USA), 'M5V 2T6' (Canada), 'SW1A 1AA' (UK), '2000' (Australia).");

            builder.Property(a => a.Country)
                .IsRequired()
                .HasColumnName("country")
                .HasMaxLength(100)
                .HasComment("The country where the address is located. This required field helps determine address formatting and timezone derivation. Examples include: 'United States', 'Canada', 'United Kingdom', 'Australia'. Use the full country name rather than abbreviations or codes.");

            builder.Property(a => a.Location)
                .IsRequired()
                .HasColumnName("location")
                .HasColumnType("geometry(Point, 4326)")
                .HasComment("The geographical coordinates (latitude and longitude) of the address. Used for mapping and location-based features. This uses NetTopologySuite.Geometries.Point to store geographic coordinates. Example: new Point(-87.6298, 41.8781) { SRID = 4326 } for Chicago.");

            builder.HasOne(a => a.Venue)
                .WithOne(v => v.Address)
                .HasForeignKey<Venue>(v => v.AddressId)
                .HasConstraintName("fk_venues_address_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
