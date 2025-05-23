namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;
    using Pulse.Data.Entities;
    using System.Linq;

    public class BusinessHoursConfiguration : IEntityTypeConfiguration<BusinessHours>
    {
        public void Configure(EntityTypeBuilder<BusinessHours> builder)
        {
            #region Entity Configuration
            builder.HasKey(bh => bh.Id);

            builder.Property(bh => bh.VenueId)
                   .IsRequired();

            builder.Property(bh => bh.DayOfWeekId)
                   .IsRequired();

            builder.Property(bh => bh.IsClosed)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne(bh => bh.Venue)
                   .WithMany(v => v.BusinessHours)
                   .HasForeignKey(bh => bh.VenueId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bh => bh.DayOfWeek)
                   .WithMany(dow => dow.BusinessHours)
                   .HasForeignKey(bh => bh.DayOfWeekId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(bh => new { bh.VenueId, bh.DayOfWeekId })
                   .IsUnique();
            #endregion

            #region Data Seed
            builder.HasData(
                #region Bullfrog Brewery Hours (Venue ID 1)
                new BusinessHours
                {                    Id = 1,
                    VenueId = 1,
                    DayOfWeekId = 1, // Sunday
                    OpenTime = new LocalTime(10, 0),
                    CloseTime = new LocalTime(15, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 2,
                    VenueId = 1,
                    DayOfWeekId = 2, // Monday
                    OpenTime = new LocalTime(11, 30),
                    CloseTime = new LocalTime(22, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 3,
                    VenueId = 1,
                    DayOfWeekId = 3, // Tuesday
                    OpenTime = new LocalTime(11, 30),
                    CloseTime = new LocalTime(22, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 4,
                    VenueId = 1,
                    DayOfWeekId = 4, // Wednesday
                    OpenTime = new LocalTime(11, 30),
                    CloseTime = new LocalTime(22, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 5,
                    VenueId = 1,
                    DayOfWeekId = 5, // Thursday
                    OpenTime = new LocalTime(11, 30),
                    CloseTime = new LocalTime(22, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 6,
                    VenueId = 1,
                    DayOfWeekId = 6, // Friday
                    OpenTime = new LocalTime(11, 30),
                    CloseTime = new LocalTime(0, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {                    Id = 7,
                    VenueId = 1,
                    DayOfWeekId = 7, // Saturday
                    OpenTime = new LocalTime(11, 30),
                    CloseTime = new LocalTime(0, 0),
                    IsClosed = false
                },
                #endregion

                #region The Brickyard Restaurant & Ale House Hours (Venue ID 2)
                new BusinessHours
                {
                    Id = 8,
                    VenueId = 2,
                    DayOfWeekId = 1, // Sunday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(23, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 9,
                    VenueId = 2,
                    DayOfWeekId = 2, // Monday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(0, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 10,
                    VenueId = 2,
                    DayOfWeekId = 3, // Tuesday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(0, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 11,
                    VenueId = 2,
                    DayOfWeekId = 4, // Wednesday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(0, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 12,
                    VenueId = 2,
                    DayOfWeekId = 5, // Thursday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(0, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 13,
                    VenueId = 2,
                    DayOfWeekId = 6, // Friday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(2, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {                    Id = 14,
                    VenueId = 2,
                    DayOfWeekId = 7, // Saturday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(2, 0),
                    IsClosed = false
                },
                #endregion

                #region The Crooked Goose Hours (Venue ID 3)
                new BusinessHours
                {
                    Id = 15,
                    VenueId = 3,
                    DayOfWeekId = 1, // Sunday
                    OpenTime = new LocalTime(10, 0),
                    CloseTime = new LocalTime(14, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 16,
                    VenueId = 3,
                    DayOfWeekId = 2, // Monday - Closed
                    OpenTime = null,
                    CloseTime = null,
                    IsClosed = true
                }, 
                new BusinessHours
                {
                    Id = 17,
                    VenueId = 3,
                    DayOfWeekId = 3, // Tuesday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(21, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 18,
                    VenueId = 3,
                    DayOfWeekId = 4, // Wednesday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(21, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 19,
                    VenueId = 3,
                    DayOfWeekId = 5, // Thursday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(21, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 20,
                    VenueId = 3,
                    DayOfWeekId = 6, // Friday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(22, 0),
                    IsClosed = false
                }, 
                new BusinessHours
                {
                    Id = 21,
                    VenueId = 3,
                    DayOfWeekId = 7, // Saturday
                    OpenTime = new LocalTime(11, 0),
                    CloseTime = new LocalTime(22, 0),
                    IsClosed = false
                }
                #endregion
            );
            #endregion
        }
    }
}
