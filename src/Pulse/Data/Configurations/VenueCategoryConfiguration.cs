namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Pulse.Data.Entities;

    public class VenueCategoryConfiguration : IEntityTypeConfiguration<VenueCategory>
    {
        public void Configure(EntityTypeBuilder<VenueCategory> builder)
        {
            builder.HasKey(vc => vc.Id);

            builder.Property(vc => vc.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(vc => vc.Description)
                   .HasMaxLength(200);
                   
            builder.Property(vc => vc.Icon)
                   .HasMaxLength(10);

            builder.HasMany(vc => vc.Venues)
                   .WithOne(v => v.Category)
                   .HasForeignKey(v => v.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(vc => vc.Name)
                   .IsUnique();
            builder.HasIndex(vc => vc.BitMask)
                   .IsUnique();

            builder.HasData(
                new VenueCategory 
                { 
                    Id = 1, 
                    Name = "Restaurant", 
                    Description = "Dining establishments offering food and beverages", 
                    Icon = "🍽️",
                    SortOrder = 1,
                    BitMask = 1
                },
                new VenueCategory 
                { 
                    Id = 2, 
                    Name = "Bar", 
                    Description = "Venues focused on drinks and nightlife", 
                    Icon = "🍸",
                    SortOrder = 2,
                    BitMask = 2
                },
                new VenueCategory 
                { 
                    Id = 3, 
                    Name = "Cafe", 
                    Description = "Casual spots for coffee and light meals", 
                    Icon = "☕",
                    SortOrder = 3,
                    BitMask = 4
                },
                new VenueCategory 
                { 
                    Id = 4, 
                    Name = "Nightclub", 
                    Description = "Venues for dancing and late-night entertainment", 
                    Icon = "🪩",
                    SortOrder = 4,
                    BitMask = 8
                },
                new VenueCategory 
                { 
                    Id = 5, 
                    Name = "Pub", 
                    Description = "Casual venues with food, drinks, and often live music", 
                    Icon = "🍺",
                    SortOrder = 5,
                    BitMask = 16
                },
                new VenueCategory 
                { 
                    Id = 6, 
                    Name = "Winery", 
                    Description = "Venues producing wine, offering tastings, food pairings, and live music", 
                    Icon = "🍷",
                    SortOrder = 6,
                    BitMask = 32
                },
                new VenueCategory 
                { 
                    Id = 7, 
                    Name = "Brewery", 
                    Description = "Venues brewing their own beer, often with food and live music", 
                    Icon = "🍻",
                    SortOrder = 7,
                    BitMask = 64
                },
                new VenueCategory 
                { 
                    Id = 9, 
                    Name = "Lounge", 
                    Description = "Sophisticated venues with cocktails, small plates, and live music", 
                    Icon = "🛋️",
                    SortOrder = 8,
                    BitMask = 128
                },
                new VenueCategory 
                { 
                    Id = 10, 
                    Name = "Bistro", 
                    Description = "Intimate dining venues with quality food, wine, and occasional live music", 
                    Icon = "🥂",
                    SortOrder = 9,
                    BitMask = 256
                }
            );
        }
    }
}
