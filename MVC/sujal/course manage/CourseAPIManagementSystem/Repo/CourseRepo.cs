using CourseAPIManagementSystem.Model;

namespace CourseAPIManagementSystem.Repo
{
    public class CourseRepo
    {
        private readonly List<Course> courses = new List<Course>
        {
            new Course{ Id=1,CourseName="C# Basics",Description="learn C# from youTube",DurationInHour=50},
            new Course{ Id=2,CourseName="JAVA ADVANCE",Description="learn JAVA ADVANCE from youTube",DurationInHour=60},
            new Course{ Id=3,CourseName="Python",Description="learn PYTHON from Google",DurationInHour=40}
        };

        public IEnumerable<Course> GetAll() => courses;

        public Course? GetbyId(int id) => courses.FirstOrDefault(c => c.Id == id);

        public void Add(Course course) 
        {
            course.Id = courses.Any() ? courses.Max(c => c.Id) + 1 : 1 ;
            courses.Add(course); 
        }

        public void Update(Course course)
        {
            var existing = GetbyId(course.Id);
            if(existing != null)
            {
                existing.CourseName = course.CourseName;
                existing.Description = course.Description;
                existing.DurationInHour = course.DurationInHour;
            }

        }


        public void Delete(int id)
        {
            var course = GetbyId(id);
            if(course != null)
            {
                courses.Remove(course);

            }
            
        }

        public IEnumerable<Course> SearchByName(string keyword)
        {
            return courses.Where(c =>c.CourseName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }

}
