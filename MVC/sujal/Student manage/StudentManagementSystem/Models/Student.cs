namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string age { get; set; }

        public string Address { get; set; }

        public string Semester { get; set; }

        public string Role { get; set; } = "Student";
    }
}
