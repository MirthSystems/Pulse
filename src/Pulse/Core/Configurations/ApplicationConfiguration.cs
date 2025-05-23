namespace Pulse.Core.Configurations
{
    public class ApplicationConfiguration
    {
        public required string ConnectionString { get; set; }

        public required AzureMaps AzureMaps { get; set; }
    }
}
