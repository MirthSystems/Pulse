namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class CreateVenueRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Url]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Url]
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        [Required]
        public CreateAddressRequest Address { get; set; } = new CreateAddressRequest();

        [Required]
        [MinLength(1, ErrorMessage = "At least one operating schedule must be provided.")]
        public ICollection<CreateOperatingScheduleRequest> BusinessHours { get; set; } = new List<CreateOperatingScheduleRequest>();
    }
}
