using StudentAPI.Models;
using System.Collections.Generic;
using System.Linq;
namespace StudentAPI.Data
{
    public class StudentRepository: IStudentRepository 
    {
        private readonly List<Student> _students = new();
        public IEnumerable<Student> GetAll()
        {
            return _students;
        }
        public Student? GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id== id);
        }

        public void Add(Student student)
        {
            student.Id= _students.Count>0 ?_students.Max(s =>s.Id) + 1 :1;
            _students.Add(student);
        }
        public void Update(Student student)
        {
            var existing = GetById(student.Id);
            if(existing != null)
            {
                existing.Name = student.Name;
                existing.Address = student.Address;
                existing.Email = student.Email;
                existing.Phone = student.Phone;
                existing.Age = student.Age;
            }
        }
        public void Delete(int id)
        {
            var student = GetById(id);
            if(student != null)
            {
                _students.Remove(student);
            }
        }
    }
}
