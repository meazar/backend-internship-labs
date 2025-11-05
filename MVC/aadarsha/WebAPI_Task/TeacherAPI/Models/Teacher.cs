namespace TeacherAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public string? FullName { get; set; }
        public string? Subject { get; set; }
        public int YearsOfExperience { get; set; }
        public string? Email { get; set; }
    }
}
