namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class GetSpecialsRequest
    {
        [Required]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; } = string.Empty;

        [Range(0.1, 100)]
        public double Radius { get; set; } = 5;

        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$", ErrorMessage = "Invalid date-time format. Use ISO 8601 format (e.g., 2025-05-05T18:30:00Z).")]
        public string SearchDateTime { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
        public string SearchTerm { get; set; } = string.Empty;

        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "SpecialTypeId must be a positive number.")]
        public int? SpecialTypeId { get; set; }

        public bool? IsCurrentlyRunning { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}
