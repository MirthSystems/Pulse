namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Pulse.Data.Entities;

    public class SpecialCategoryConfiguration : IEntityTypeConfiguration<SpecialCategory>
    {
        public void Configure(EntityTypeBuilder<SpecialCategory> builder)
        {
            #region Entity Configuration
            builder.ToTable("special_categories");
            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(sc => sc.Description)
                   .HasMaxLength(200);

            builder.Property(sc => sc.Icon)
                   .HasMaxLength(10);

            builder.HasIndex(sc => sc.Name)
                .IsUnique();
            #endregion

            #region Data Seed
            builder.HasData(
                new SpecialCategory
                {
                    Id = 1,
                    Name = "Food",
                    Description = "Food specials, appetizers, and meal deals",
                    Icon = "🍔",
                    BitMask = 1,
                    SortOrder = 1,
                },
                new SpecialCategory
                {
                    Id = 2,
                    Name = "Drink",
                    Description = "Drink specials, happy hours, and beverage promotions",
                    Icon = "🍺",
                    BitMask = 2,
                    SortOrder = 2,
                },
                new SpecialCategory
                {
                    Id = 3,
                    Name = "Entertainment",
                    Description = "Live music, DJs, trivia, karaoke, and other events",
                    Icon = "🎵",
                    BitMask = 4,
                    SortOrder = 3,
                }
            );
            #endregion
        }
    }
}
