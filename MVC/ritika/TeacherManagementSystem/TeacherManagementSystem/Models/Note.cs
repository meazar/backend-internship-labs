using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TeacherPortalMvc.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string FileName { get; set; }

        public DateTime UploadedAt { get; set; }

        [Required]
        public IFormFile? File { get; set; }
    }
}
