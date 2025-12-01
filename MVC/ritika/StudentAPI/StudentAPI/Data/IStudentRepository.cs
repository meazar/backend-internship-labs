using StudentAPI.Models;
using System.Collections.Generic;

namespace StudentAPI.Data
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
        Student? GetById(int id);
        void Add(Student student);
        void Update(Student student);
        void Delete(int id);

    }
}
