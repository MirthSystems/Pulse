namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing tag
    /// </summary>
    public class UpdateTagRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
    }
}
