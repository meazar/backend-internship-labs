using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities
{
    public class Author : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Biography { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateOnly? DateOfDeath { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
