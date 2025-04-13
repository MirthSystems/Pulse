namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new tag
    /// </summary>
    public class NewTagRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
