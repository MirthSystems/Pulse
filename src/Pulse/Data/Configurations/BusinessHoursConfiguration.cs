namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Pulse.Data.Entities;

    public class BusinessHoursConfiguration : IEntityTypeConfiguration<BusinessHours>
    {
        public void Configure(EntityTypeBuilder<BusinessHours> builder)
        {
            builder.HasKey(bh => bh.Id);

            builder.HasOne(bh => bh.Venue)
                   .WithMany(v => v.BusinessHours)
                   .HasForeignKey(bh => bh.VenueId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bh => bh.DayOfWeek)
                   .WithMany(dow => dow.BusinessHours)
                   .HasForeignKey(bh => bh.DayOfWeekId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(bh => new { bh.VenueId, bh.DayOfWeekId }).IsUnique();
        }
    }
}
