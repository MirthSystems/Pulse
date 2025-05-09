namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new operating schedule entry for a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines when a venue is open on a specific day of the week.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Used both as a standalone API request and within venue creation requests.</para>
    /// </remarks>
    public class CreateOperatingScheduleRequest : OperatingHours
    {
        /// <summary>
        /// Gets or sets the ID of the venue this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>This field is required for standalone operating schedule creation requests.</para>
        /// <para>When used within a CreateVenueRequest, this field is ignored as the venue ID is not yet available.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;
    }
}
