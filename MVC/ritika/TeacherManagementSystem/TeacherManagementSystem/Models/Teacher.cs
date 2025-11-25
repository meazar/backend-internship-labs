using System.ComponentModel.DataAnnotations;
namespace TeacherPortalMvc.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string Name {  get; set; }
        [Range(22,70)]
        public int Age { get; set; }
        [Range(10000,100000)]
        public int Salary { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
