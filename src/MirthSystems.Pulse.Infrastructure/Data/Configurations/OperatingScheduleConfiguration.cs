namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class OperatingScheduleConfiguration : IEntityTypeConfiguration<OperatingSchedule>
    {
        public void Configure(EntityTypeBuilder<OperatingSchedule> builder)
        {
            builder.ToTable("operating_schedules");

            builder.HasKey(os => os.Id);

            builder.Property(os => os.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the operating schedule.");

            builder.Property(os => os.VenueId)
                .HasColumnName("venue_id")
                .IsRequired()
                .HasComment("The foreign key to the Venue.");

            builder.Property(os => os.DayOfWeek)
                .HasColumnName("day_of_week")
                .IsRequired()
                .HasComment("The day of the week (0=Sunday, 1=Monday, etc.).");

            builder.Property(os => os.TimeOfOpen)
                .HasColumnName("time_of_open")
                .IsRequired()
                .HasComment("The opening time, stored as local time.");

            builder.Property(os => os.TimeOfClose)
                .HasColumnName("time_of_close")
                .IsRequired()
                .HasComment("The closing time, stored as local time.");

            builder.Property(os => os.IsClosed)
                .HasColumnName("is_closed")
                .IsRequired()
                .HasComment("Indicates if the venue is closed on this day.");

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
