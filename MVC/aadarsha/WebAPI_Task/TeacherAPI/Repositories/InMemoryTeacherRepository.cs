using System.Collections.Generic;
using System.Linq;
using TeacherAPI.Models;

namespace TeacherAPI.Repositories
{
    public class InMemoryTeacherRepository
    {
        private readonly Dictionary<int, Teacher> _teachers = new();
        private int _nextId = 1;

        public InMemoryTeacherRepository()
        {
            AddTeacher(new Teacher { FullName = "Lionel Messi", Subject = "Mathematics", YearsOfExperience = 20, Email = "messi@example.com" });
            AddTeacher(new Teacher { FullName = "Cristiano Ronaldo", Subject = "Physics", YearsOfExperience = 23, Email = "ronaldo@example.com" });
            AddTeacher(new Teacher { FullName = "Yamal", Subject = "Chemistry", YearsOfExperience = 4, Email = "yamal@example.com" });
        }

        // GET ALL TEACHERS
        public IEnumerable<Teacher> GetTeachersAll() => _teachers.Values;

        // GET TEACHER BY ID
        public Teacher? GetTeacherById(int id)
        {
            _teachers.TryGetValue(id, out var teacher);
            return teacher;
        }

        // CREATE TEACHER
        public Teacher AddTeacher(Teacher teacher)
        {
            teacher.Id = _nextId++;
            _teachers[teacher.Id] = teacher;
            return teacher;
        }

        // UPDATE TEACHER
        public bool UpdateTeacher(int id, Teacher updatedTeacher)
        {
            if (!_teachers.ContainsKey(id))
                return false;

            updatedTeacher.Id = id;
            _teachers[id] = updatedTeacher;
            return true;
        }

        // DELETE TEACHER
        public bool DeleteTeacher(int id)
        {
            return _teachers.Remove(id);
        }
    }
}