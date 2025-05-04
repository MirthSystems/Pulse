namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;
    using NodaTime;

    public class OperatingScheduleConfiguration : IEntityTypeConfiguration<OperatingSchedule>
    {
        public void Configure(EntityTypeBuilder<OperatingSchedule> builder)
        {
            builder.ToTable("operating_schedules");

            builder.HasKey(os => os.Id)
                .HasName("pk_operating_schedules");

            builder.Property(os => os.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the operating schedule entry. This is the primary key for the OperatingSchedule entity in the database.");

            builder.Property(os => os.VenueId)
                .HasColumnName("venue_id")
                .HasComment("The foreign key to the Venue entity. This represents the venue to which this operating schedule applies. This is a required field and is used to establish the relationship with the Venue entity.");

            builder.Property(os => os.DayOfWeek)
                .HasColumnName("day_of_week")
                .HasComment("The day of the week for this operating schedule entry. This uses the standard .NET System.DayOfWeek enum. The values are: Sunday (0), Monday (1), Tuesday (2), etc.");

            builder.Property(os => os.TimeOfOpen)
                .HasColumnName("time_of_open")
                .HasComment("The opening time for the venue on this day. Uses NodaTime's LocalTime to accurately represent time-of-day without date or timezone. This property is not relevant when IsClosed is true.");

            builder.Property(os => os.TimeOfClose)
                .HasColumnName("time_of_close")
                .HasComment("The closing time for the venue on this day. Uses NodaTime's LocalTime to accurately represent time-of-day without date or timezone. If this is earlier than TimeOfOpen, it's interpreted as closing after midnight (the next day). This property is not relevant when IsClosed is true.");

            builder.Property(os => os.IsClosed)
                .HasColumnName("is_closed")
                .HasComment("A value indicating whether the venue is closed on this day. When true, the venue is completely closed on this day of the week. When false, the venue is open according to the TimeOfOpen and TimeOfClose properties.");

            builder.HasOne(os => os.Venue)
                .WithMany(v => v.BusinessHours)
                .HasForeignKey(os => os.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(os => new { os.VenueId, os.DayOfWeek })
                .IsUnique()
                .HasDatabaseName("ix_operating_schedules_venue_id_day_of_week");
        }
    }
}
