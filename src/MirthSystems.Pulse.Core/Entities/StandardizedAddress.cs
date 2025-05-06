namespace MirthSystems.Pulse.Core.Entities
{
    /// <summary>
    /// Represents the result of address standardization, containing parsed address components.
    /// </summary>
    /// <remarks>
    /// <para>This class holds the components of a standardized address as parsed by PostgreSQL's address_standardizer extension.</para>
    /// <para>Each property represents a specific part of a parsed address according to USPS addressing standards.</para>
    /// <para>Used primarily to standardize address formats and to support geocoding operations.</para>
    /// </remarks>
    public class StandardizedAddress
    {
        /// <summary>
        /// Gets or sets the building name or identifier.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Empire State Building"</para>
        /// <para>- "Willis Tower"</para>
        /// <para>- "Transamerica Pyramid"</para>
        /// </remarks>
        public string? Building { get; set; }
        
        /// <summary>
        /// Gets or sets the house or street number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "123" (in "123 Main St")</para>
        /// <para>- "1600" (in "1600 Pennsylvania Ave")</para>
        /// </remarks>
        public string? HouseNum { get; set; }
        
        /// <summary>
        /// Gets or sets the directional prefix (pre-directional).
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "N" (in "N Main St")</para>
        /// <para>- "W" (in "W 42nd St")</para>
        /// <para>- "SW" (in "SW 3rd Ave")</para>
        /// </remarks>
        public string? PreDir { get; set; }
        
        /// <summary>
        /// Gets or sets the qualifier for the address.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Old" (in "Old Highway 52")</para>
        /// <para>- "Upper" (in "Upper Broadway")</para>
        /// </remarks>
        public string? Qual { get; set; }
        
        /// <summary>
        /// Gets or sets the street type prefix.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Avenue" (in "Avenue of the Stars")</para>
        /// <para>- "Highway" (in "Highway 101")</para>
        /// </remarks>
        public string? PreType { get; set; }
        
        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Main" (in "123 Main St")</para>
        /// <para>- "Broadway" (in "1500 Broadway")</para>
        /// <para>- "Pennsylvania" (in "1600 Pennsylvania Ave")</para>
        /// </remarks>
        public string? Name { get; set; }
        
        /// <summary>
        /// Gets or sets the street type suffix.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "St" (Street)</para>
        /// <para>- "Ave" (Avenue)</para>
        /// <para>- "Blvd" (Boulevard)</para>
        /// <para>- "Rd" (Road)</para>
        /// </remarks>
        public string? SufType { get; set; }
        
        /// <summary>
        /// Gets or sets the directional suffix (post-directional).
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "N" (in "18th St N")</para>
        /// <para>- "SE" (in "Broadway SE")</para>
        /// </remarks>
        public string? SufDir { get; set; }
        
        /// <summary>
        /// Gets or sets the rural route identifier.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "RR 2" (Rural Route 2)</para>
        /// <para>- "HC 65" (Highway Contract 65)</para>
        /// </remarks>
        public string? RuralRoute { get; set; }
        
        /// <summary>
        /// Gets or sets additional address information not fitting other categories.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Corner of" (in "Corner of Main and Broadway")</para>
        /// <para>- "Near" (in "Near Central Park")</para>
        /// </remarks>
        public string? Extra { get; set; }
        
        /// <summary>
        /// Gets or sets the city or locality name.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "New York"</para>
        /// <para>- "San Francisco"</para>
        /// </remarks>
        public string? City { get; set; }
        
        /// <summary>
        /// Gets or sets the state or region.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "IL" (Illinois)</para>
        /// <para>- "NY" (New York)</para>
        /// <para>- "CA" (California)</para>
        /// </remarks>
        public string? State { get; set; }
        
        /// <summary>
        /// Gets or sets the country name or code.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "US" (United States)</para>
        /// <para>- "CA" (Canada)</para>
        /// <para>- "UK" (United Kingdom)</para>
        /// </remarks>
        public string? Country { get; set; }
        
        /// <summary>
        /// Gets or sets the postal or ZIP code.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "60601" (Chicago)</para>
        /// <para>- "10001" (New York)</para>
        /// <para>- "94103" (San Francisco)</para>
        /// </remarks>
        public string? Postcode { get; set; }
        
        /// <summary>
        /// Gets or sets the Post Office box number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "PO Box 123"</para>
        /// <para>- "Box 456"</para>
        /// </remarks>
        public string? Box { get; set; }
        
        /// <summary>
        /// Gets or sets the unit or apartment number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Apt 301"</para>
        /// <para>- "Suite 200"</para>
        /// <para>- "Unit 4B"</para>
        /// </remarks>
        public string? Unit { get; set; }
    }
}
