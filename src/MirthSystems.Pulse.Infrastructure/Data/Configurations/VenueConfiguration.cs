namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using MirthSystems.Pulse.Core.Entities;

    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.ToTable("venues");

            builder.HasKey(v => v.Id)
                .HasName("pk_venues");

            builder.Property(v => v.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the venue. This is the primary key for the venue entity in the database.");

            builder.Property(v => v.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(255)
                .HasComment("This required field provides the primary identifier for the venue as displayed to users. Examples of venue names include: 'The Rusty Anchor Pub', 'Downtown Music Hall', 'Cafe Milano'.");

            builder.Property(v => v.Description)
                .HasColumnName("description")
                .HasMaxLength(1000)
                .HasComment("This optional field provides an overview of the venue's offerings or unique features. Examples include: 'A cozy pub with live music and craft beers.', 'A classic diner serving daily specials and homestyle meals.'");

            builder.Property(v => v.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .HasComment("This optional field allows users to contact the venue directly and supports international formats. Examples include: '+1 (555) 123-4567' (USA), '+44 20 7946 0958' (UK), '+61 2 9876 5432' (Australia).");

            builder.Property(v => v.Website)
                .HasColumnName("website")
                .HasMaxLength(255)
                .HasComment("This optional field provides a link to the venue's online presence. Examples include: 'https://www.rustyanchorpub.com', 'https://downtownmusichall.com'. Always include the full URL including the protocol.");

            builder.Property(v => v.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .HasComment("This optional field allows for direct email contact with the venue. Examples include: 'info@rustyanchorpub.com', 'contact@downtownmusichall.com'.");

            builder.Property(v => v.ProfileImage)
                .HasColumnName("profile_image")
                .HasMaxLength(255)
                .HasComment("URL to the venue's profile image (square format). Examples include: 'https://cdn.pulse.com/venues/123456/profile.jpg'. Recommended image specs: 512x512 pixels, square aspect ratio, JPG or PNG format.");

            builder.Property(v => v.AddressId)
                .HasColumnName("address_id")
                .HasComment("The foreign key to the Address entity. This represents the physical address where the venue is located. This is a required field and is used to establish the relationship with the Address entity.");

            builder.Property(v => v.CreatedAt)
                .HasColumnName("created_at")
                .HasComment("The timestamp when the venue was created. Example: '2023-01-01T08:00:00Z' for a venue created on January 1, 2023.");

            builder.Property(v => v.CreatedByUserId)
                .IsRequired()
                .HasColumnName("created_by_user_id")
                .HasMaxLength(100)
                .HasComment("The ID of the user who created the venue. Example: 'auth0|12345' for the user who created the venue.");

            builder.Property(v => v.UpdatedAt)
                .HasColumnName("updated_at")
                .HasComment("The timestamp when the venue was last updated, if applicable. Example: '2023-02-15T10:00:00Z' for a venue updated on February 15, 2023.");

            builder.Property(v => v.UpdatedByUserId)
                .HasColumnName("updated_by_user_id")
                .HasMaxLength(100)
                .HasComment("The ID of the user who last updated the venue, if applicable. Example: 'auth0|12345' for the user who last updated the venue.");

            builder.Property(v => v.IsDeleted)
                .HasColumnName("is_deleted")
                .HasComment("Whether the venue has been soft-deleted. Default is false. When true, the venue is considered deleted but remains in the database for auditing purposes.");

            builder.Property(v => v.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the venue was deleted, if applicable. Example: '2023-03-01T12:00:00Z' for a venue deleted on March 1, 2023.");

            builder.Property(v => v.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasMaxLength(100)
                .HasComment("The ID of the user who deleted the venue, if applicable. Example: 'auth0|12345' for the user who performed the deletion.");

            builder.HasOne(v => v.Address)
                .WithOne(a => a.Venue)
                .HasForeignKey<Venue>(v => v.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.BusinessHours)
                .WithOne(os => os.Venue)
                .HasForeignKey(os => os.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.Specials)
                .WithOne(s => s.Venue)
                .HasForeignKey(s => s.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
