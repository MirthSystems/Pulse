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

            builder.HasIndex(vc => vc.Name).IsUnique();

            builder.HasMany(vc => vc.Venues)
                   .WithOne(v => v.Category)
                   .HasForeignKey(v => v.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new VenueCategory 
                { 
                    Id = 1, 
                    Name = "Restaurant", 
                    Description = "Dining establishments offering food and beverages", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 2, 
                    Name = "Bar", 
                    Description = "Venues focused on drinks and nightlife", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 3, 
                    Name = "Cafe", 
                    Description = "Casual spots for coffee and light meals", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 4, 
                    Name = "Nightclub", 
                    Description = "Venues for dancing and late-night entertainment", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 5, 
                    Name = "Pub", 
                    Description = "Casual venues with food, drinks, and often live music", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 6, 
                    Name = "Winery", 
                    Description = "Venues producing wine, offering tastings, food pairings, and live music", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 7, 
                    Name = "Brewery", 
                    Description = "Venues brewing their own beer, often with food and live music", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 8, 
                    Name = "Taproom", 
                    Description = "Venues serving craft beer, often with food trucks and live music", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 9, 
                    Name = "Lounge", 
                    Description = "Sophisticated venues with cocktails, small plates, and live music", 
                    IsActive = true 
                },
                new VenueCategory 
                { 
                    Id = 10, 
                    Name = "Bistro", 
                    Description = "Intimate dining venues with quality food, wine, and occasional live music", 
                    IsActive = true 
                }
            );
        }
    }
}
