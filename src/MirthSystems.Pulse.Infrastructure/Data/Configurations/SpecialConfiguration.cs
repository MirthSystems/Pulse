namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using MirthSystems.Pulse.Core.Entities;

    public class SpecialConfiguration : IEntityTypeConfiguration<Special>
    {
        public void Configure(EntityTypeBuilder<Special> builder)
        {
            builder.ToTable("specials");

            builder.HasKey(s => s.Id)
                .HasName("pk_specials");

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the special. This is the primary key for the Special entity in the database.");

            builder.Property(s => s.VenueId)
                .HasColumnName("venue_id")
                .HasComment("The foreign key to the Venue entity. This represents the venue to which this special applies.");

            builder.Property(s => s.Content)
                .IsRequired()
                .HasColumnName("content")
                .HasMaxLength(500)
                .HasComment("A brief description of the special. This required field provides details about what the special entails, making it clear to users what is being offered. Examples include: 'Half-Price Wings Happy Hour', 'Live Jazz Night', 'Buy One Get One Free Draft Beer'.");

            builder.Property(s => s.Type)
                .HasColumnName("type")
                .HasComment("The category of the special, such as Food, Drink, or Entertainment. This required field helps classify and filter specials for users. Examples include: SpecialTypes.Food, SpecialTypes.Drink, SpecialTypes.Entertainment.");

            builder.Property(s => s.StartDate)
                .HasColumnName("start_date")
                .HasComment("The start date of the special. For one-time specials, this is the event date. For recurring specials, this is the first occurrence. The date is interpreted in the venue's timezone.");

            builder.Property(s => s.StartTime)
                .HasColumnName("start_time")
                .HasComment("The start time of the special on the start date or each recurrence. This required field is interpreted in the venue's timezone. Examples include: 8 PM concert: LocalTime(20, 0), 5 PM happy hour: LocalTime(17, 0).");

            builder.Property(s => s.EndTime)
                .HasColumnName("end_time")
                .HasComment("The end time of the special on the same day, or null if it spans multiple days or has no daily end. This optional field is interpreted in the venue's timezone. Examples include: 10 PM concert end: LocalTime(22, 0), 7 PM happy hour end: LocalTime(19, 0), Multi-day special: null.");

            builder.Property(s => s.ExpirationDate)
                .HasColumnName("expiration_date")
                .HasComment("The expiration date of the special, or null if ongoing or same-day. For one-time specials, this is the end date if multi-day. For recurring specials, this is the last occurrence. Examples include: Multi-day sale: LocalDate(2023, 11, 3), Recurring end: LocalDate(2024, 3, 1), Same-day special: null.");

            builder.Property(s => s.IsRecurring)
                .HasColumnName("is_recurring")
                .HasComment("Determines whether the special repeats over time. If false, the special is a one-time event. If true, it recurs according to the CronSchedule. Examples include: One-time concert: false, Weekly happy hour: true.");

            builder.Property(s => s.CronSchedule)
                .HasColumnName("cron_schedule")
                .HasMaxLength(100)
                .HasComment("A CRON expression defining the recurrence schedule for the special, or null if not recurring. Common examples: Daily at 5 PM: '0 17 * * *', Every Monday at 8 PM: '0 20 * * 1', Weekdays at 4 PM: '0 16 * * 1-5'.");

            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .HasComment("The timestamp when the special was created. Example: '2023-04-01T09:00:00Z' for a special created on April 1, 2023.");

            builder.Property(s => s.CreatedByUserId)
                .IsRequired()
                .HasColumnName("created_by_user_id")
                .HasMaxLength(100)
                .HasComment("The ID of the user who created the special. Example: 'auth0|12345' for the user who created the special.");

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .HasComment("The timestamp when the special was last updated, if applicable. Example: '2023-04-15T14:00:00Z' for a special updated on April 15, 2023.");

            builder.Property(s => s.UpdatedByUserId)
                .HasColumnName("updated_by_user_id")
                .HasMaxLength(100)
                .HasComment("The ID of the user who last updated the special, if applicable. Example: 'auth0|12345' for the user who last updated the special.");

            builder.Property(s => s.IsDeleted)
                .HasColumnName("is_deleted")
                .HasComment("Whether the special has been soft-deleted. Default is false. When true, the special is considered deleted but remains in the database for auditing purposes.");

            builder.Property(s => s.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the special was deleted, if applicable. Example: '2023-05-01T12:00:00Z' for a special deleted on May 1, 2023.");

            builder.Property(s => s.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasMaxLength(100)
                .HasComment("The ID of the user who deleted the special, if applicable. Example: 'auth0|12345' for the user who performed the deletion.");

            builder.HasOne(s => s.Venue)
                .WithMany()
                .HasForeignKey(s => s.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
