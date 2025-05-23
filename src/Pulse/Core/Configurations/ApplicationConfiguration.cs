namespace Pulse.Core.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ApplicationConfiguration
    {
        public required string ConnectionString { get; set; }

        public required AzureMaps AzureMaps { get; set; }
    }

    public class ApplicationConfigurationConfiguration : IEntityTypeConfiguration<ApplicationConfiguration>
    {
        public void Configure(EntityTypeBuilder<ApplicationConfiguration> builder)
        {
            builder.Property(ac => ac.ConnectionString)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
