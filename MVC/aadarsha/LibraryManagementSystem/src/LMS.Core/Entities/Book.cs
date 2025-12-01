using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities;


public class Book : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? ISBN { get; set; }
        
        [Required]
        public int TotalCopies { get; set; } = 1;
        
        [Required]
        public int AvailableCopies { get; set; } = 1;
        
        public bool IsActive { get; set; } = true;
    }
