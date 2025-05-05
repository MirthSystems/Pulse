namespace MirthSystems.Pulse.Core.DataTransferObjects
{
    using Azure.Core.GeoJson;

    public class AddressDataTransferObject
    {
        /// <summary>
        /// The unique identifier for the address
        /// </summary>
        /// <remarks>e.g. 1</remarks>
        public long Id { get; set; }

        /// <summary>
        /// The primary street address information
        /// </summary>
        /// <remarks>e.g. 123 Main Street</remarks>
        public required string StreetAddress { get; set; }

        /// <summary>
        /// Additional address details like unit numbers
        /// </summary>
        /// <remarks>e.g. Suite 200</remarks>
        public string? SecondaryAddress { get; set; }

        /// <summary>
        /// The city or town
        /// </summary>
        /// <remarks>e.g. Springfield</remarks>
        public required string Locality { get; set; }

        /// <summary>
        /// The state, province, or region
        /// </summary>
        /// <remarks>e.g. Illinois</remarks>
        public required string Region { get; set; }

        /// <summary>
        /// The postal or ZIP code
        /// </summary>
        /// <remarks>e.g. 62701</remarks>
        public required string Postcode { get; set; }

        /// <summary>
        /// The country
        /// </summary>
        /// <remarks>e.g. United States</remarks>
        public required string Country { get; set; }

        /// <summary>
        /// The geographical point representing the address location
        /// </summary>
        public GeoPoint? Location { get; set; }
    }
}
