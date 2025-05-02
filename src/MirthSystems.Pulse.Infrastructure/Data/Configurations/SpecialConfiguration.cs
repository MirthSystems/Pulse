namespace MirthSystems.Pulse.Infrastructure.Data.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using MirthSystems.Pulse.Core.Models.Entities;

    public class SpecialConfiguration : IEntityTypeConfiguration<Special>
    {
        public void Configure(EntityTypeBuilder<Special> builder)
        {
            builder.ToTable("specials");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .HasComment("The unique identifier for the special.");

            builder.Property(s => s.VenueId)
                .HasColumnName("venue_id")
                .IsRequired()
                .HasComment("The foreign key to the Venue.");

            builder.Property(s => s.Content)
                .HasColumnName("content")
                .HasMaxLength(500)
                .IsRequired()
                .HasComment("The description of the special, e.g., 'Half-Price Wings'.");

            builder.Property(s => s.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasComment("The type of special (Food=0, Drink=1, Entertainment=2).");

            builder.Property(s => s.StartDate)
                .HasColumnName("start_date")
                .IsRequired()
                .HasComment("The start date of the special, stored as a local date.");

            builder.Property(s => s.StartTime)
                .HasColumnName("start_time")
                .IsRequired()
                .HasComment("The start time of the special, stored as a local time.");

            builder.Property(s => s.EndTime)
                .HasColumnName("end_time")
                .HasComment("The end time of the special, if applicable.");

            builder.Property(s => s.ExpirationDate)
                .HasColumnName("expiration_date")
                .HasComment("The expiration date of the special, if applicable.");

            builder.Property(s => s.IsRecurring)
                .HasColumnName("is_recurring")
                .IsRequired()
                .HasComment("Indicates if the special is recurring.");

            builder.Property(s => s.CronSchedule)
                .HasColumnName("cron_schedule")
                .HasMaxLength(100)
                .HasComment("The CRON expression for recurring specials, e.g., '0 17 * * *'.");

            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasComment("The timestamp when the special was created.");

            builder.Property(s => s.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired()
                .HasComment("The ID of the user who created the special.");

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .HasComment("The timestamp when the special was last updated, if applicable.");

            builder.Property(s => s.UpdatedByUserId)
                .HasColumnName("updated_by_user_id")
                .HasComment("The ID of the user who last updated the special, if applicable.");

            builder.Property(s => s.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .HasComment("Indicates if the special has been soft-deleted.");

            builder.Property(s => s.DeletedAt)
                .HasColumnName("deleted_at")
                .HasComment("The timestamp when the special was deleted, if applicable.");

            builder.Property(s => s.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasComment("The ID of the user who deleted the special, if applicable.");

            builder.HasOne(s => s.Venue)
                .WithMany()
                .HasForeignKey(s => s.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.CreatedByUser)
                .WithMany(u => u.CreatedSpecials)
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.UpdatedByUser)
                .WithMany(u => u.UpdatedSpecials)
                .HasForeignKey(s => s.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.DeletedByUser)
                .WithMany()
                .HasForeignKey(s => s.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
