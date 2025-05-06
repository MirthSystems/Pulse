namespace MirthSystems.Pulse.Core.Models.Requests
{
    using MirthSystems.Pulse.Core.Enums;

    using System.ComponentModel.DataAnnotations;

    public class UpdateSpecialRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(SpecialTypes))]
        public SpecialTypes Type { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string ExpirationDate { get; set; } = string.Empty;

        [Required]
        public bool IsRecurring { get; set; }

        [StringLength(100)]
        public string CronSchedule { get; set; } = string.Empty;
    }
}
