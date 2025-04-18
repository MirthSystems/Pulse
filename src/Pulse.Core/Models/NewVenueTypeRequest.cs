﻿namespace Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new venue type
    /// </summary>
    public class NewVenueTypeRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
