namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using DayOfWeek = Pulse.Data.Entities.DayOfWeek;

    public class DayOfWeekConfiguration : IEntityTypeConfiguration<DayOfWeek>
    {
        public void Configure(EntityTypeBuilder<DayOfWeek> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasIndex(d => d.IsoNumber)
                .IsUnique();
            builder.HasIndex(d => d.BitMask)
                .IsUnique();

            builder.HasData(
                new DayOfWeek 
                { 
                    Id = 1, 
                    Name = "Sunday", 
                    ShortName = "SUN", 
                    IsoNumber = 0, 
                    BitMask = 1, 
                    IsWeekday = false, 
                    SortOrder = 1 
                },
                new DayOfWeek 
                { 
                    Id = 2, 
                    Name = "Monday", 
                    ShortName = "MON", 
                    IsoNumber = 1, 
                    BitMask = 2, 
                    IsWeekday = true, 
                    SortOrder = 2 
                },
                new DayOfWeek 
                { 
                    Id = 3, 
                    Name = "Tuesday", 
                    ShortName = "TUE", 
                    IsoNumber = 2, 
                    BitMask = 4, 
                    IsWeekday = true, 
                    SortOrder = 3 
                },
                new DayOfWeek 
                { 
                    Id = 4, 
                    Name = "Wednesday", 
                    ShortName = "WED", 
                    IsoNumber = 3, 
                    BitMask = 8, 
                    IsWeekday = true, 
                    SortOrder = 4 
                },
                new DayOfWeek 
                { 
                    Id = 5, 
                    Name = "Thursday", 
                    ShortName = "THU", 
                    IsoNumber = 4, 
                    BitMask = 16, 
                    IsWeekday = true, 
                    SortOrder = 5 
                },
                new DayOfWeek 
                { 
                    Id = 6, 
                    Name = "Friday", 
                    ShortName = "FRI", 
                    IsoNumber = 5, 
                    BitMask = 32, 
                    IsWeekday = true, 
                    SortOrder = 6 
                },
                new DayOfWeek 
                { 
                    Id = 7, 
                    Name = "Saturday", 
                    ShortName = "SAT", 
                    IsoNumber = 6, 
                    BitMask = 64, 
                    IsWeekday = false, 
                    SortOrder = 7 
                }
            );
        }
    }
}
