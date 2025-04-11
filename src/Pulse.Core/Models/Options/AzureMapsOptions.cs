namespace Pulse.Core.Models.Options
{
    /// <summary>
    /// Configuration options for Azure Maps service
    /// </summary>
    public class AzureMapsOptions
    {
        /// <summary>
        /// Section name in configuration file
        /// </summary>
        public const string SectionName = "AzureMaps";

        /// <summary>
        /// Azure Maps subscription key
        /// This should be stored in user secrets, not in appsettings.json
        /// </summary>
        public string SubscriptionKey { get; set; } = string.Empty;

        /// <summary>
        /// Azure Maps client ID
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Azure Maps resource group name
        /// </summary>
        public string ResourceGroup { get; set; } = string.Empty;

        /// <summary>
        /// Azure Maps account name
        /// </summary>
        public string AccountName { get; set; } = string.Empty;
    }
}
