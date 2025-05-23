namespace Pulse.Core.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AzureMaps
    {
        public required string SubscriptionKey { get; set; }
    }

    public class AzureMapsConfiguration : IEntityTypeConfiguration<AzureMaps>
    {
        public void Configure(EntityTypeBuilder<AzureMaps> builder)
        {
            builder.Property(am => am.SubscriptionKey)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
