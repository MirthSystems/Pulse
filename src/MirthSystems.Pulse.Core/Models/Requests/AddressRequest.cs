namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new address.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the required and optional fields for creating a new address entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Used primarily within venue creation requests and as a standalone API request.</para>
    /// </remarks>
    public class AddressRequest
    {
        /// <summary>
        /// Gets or sets the street address (primary address line).
        /// </summary>
        /// <remarks>
        /// <para>This should contain the street number and name.</para>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St"</para>
        /// <para>- "456 Broadway"</para>
        /// <para>- "789 Park Avenue"</para>
        /// </remarks>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secondary address information.
        /// </summary>
        /// <remarks>
        /// <para>This should contain apartment, suite, unit, building, floor, etc.</para>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200"</para>
        /// <para>- "Apt 303"</para>
        /// <para>- "Floor 15"</para>
        /// </remarks>
        [StringLength(50)]
        public string? SecondaryAddress { get; set; }

        /// <summary>
        /// Gets or sets the city or locality.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "San Francisco"</para>
        /// <para>- "Miami"</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois"</para>
        /// <para>- "California"</para>
        /// <para>- "Florida"</para>
        /// <para>You may use either the full name or the standard abbreviation (IL, CA, FL).</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal code or ZIP code.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "60601" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// </remarks>
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// <para>Use the full country name rather than abbreviations or codes.</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;
    }
}
