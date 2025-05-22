namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Pulse.Data.Entities;

    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.HasKey(v => v.Id);

            builder.HasOne(v => v.Category)
                   .WithMany(vc => vc.Venues)
                   .HasForeignKey(v => v.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.Property(v => v.IsActive).HasDefaultValue(true);
        }
    }
}
