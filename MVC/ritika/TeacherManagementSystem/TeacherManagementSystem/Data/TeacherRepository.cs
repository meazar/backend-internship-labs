using System.Collections.Generic;
using System.Linq;
using TeacherPortalMvc.Models;


namespace TeacherPortalMVC.Data
{
    public class TeacherRepository
    {
        // _teachers acts as in-memory database
        private static List<Teacher> _teachers = new();
       
        private static int _nextId = 1;

        public List<Teacher> GetAll() => _teachers;
        public Teacher GetById(int id)
        {
            return _teachers.FirstOrDefault(t => t.Id == id);
        }
        public void Add(Teacher t) 
        { 
            t.Id = _nextId++; 
            _teachers.Add(t); 
        }
        public void Update(Teacher t)
        {
            var existing = GetById(t.Id);
            if (existing != null)
            {
                existing.Name = t.Name;
                existing.Subject = t.Subject;
                existing.Age = t.Age;
                existing.Salary = t.Salary;
            }
        }
        public void Delete(int id)
        {
            var t = GetById(id);
            if (t != null) _teachers.Remove(t);
        }
    }
}
