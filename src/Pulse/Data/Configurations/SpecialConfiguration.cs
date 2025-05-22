namespace Pulse.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Pulse.Data.Entities;

    public class SpecialConfiguration : IEntityTypeConfiguration<Special>
    {
        public void Configure(EntityTypeBuilder<Special> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.Venue)
                   .WithMany(v => v.Specials)
                   .HasForeignKey(s => s.VenueId)
                   .OnDelete(DeleteBehavior.Cascade);
                   
            builder.HasOne(s => s.Category)
                   .WithMany(sc => sc.Specials)
                   .HasForeignKey(s => s.SpecialCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
