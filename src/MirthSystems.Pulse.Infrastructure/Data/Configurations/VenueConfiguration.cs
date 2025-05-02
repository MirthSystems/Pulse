namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.ToTable("venues");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the venue.");

            builder.Property(v => v.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired()
                .HasComment("The name of the venue, e.g., 'The Rusty Anchor Pub'.");

            builder.Property(v => v.Description)
                .HasColumnName("description")
                .HasMaxLength(1000)
                .HasComment("A description of the venue.");

            builder.Property(v => v.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(50)
                .HasComment("The venue's phone number, e.g., '+15551234567'.");

            builder.Property(v => v.Website)
                .HasColumnName("website")
                .HasMaxLength(200)
                .HasComment("The venue's website URL, e.g., 'https://www.example.com'.");

            builder.Property(v => v.Email)
                .HasColumnName("email")
                .HasMaxLength(100)
                .HasComment("The venue's email, e.g., 'info@example.com'.");

            builder.Property(v => v.ProfileImage)
                .HasColumnName("profile_image")
                .HasMaxLength(500)
                .HasComment("The URL to the venue's profile image.");

            builder.Property(v => v.AddressId)
                .HasColumnName("address_id")
                .IsRequired()
                .HasComment("The foreign key to the Address.");

            builder.HasOne(v => v.Address)
                .WithOne(a => a.Venue)
                .HasForeignKey<Venue>(v => v.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
