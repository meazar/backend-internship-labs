using System.Collections.Generic;
using System.Linq;
using TeacherPortalMvc.Models;


namespace TeacherPortalMVC.Data
{
    public class NoteRepository
    {
        private static List<Note> _notes = new();
        private static int _nextId = 1;

        public List<Note> GetByTeacher(int teacherId)
        {
            return _notes.Where(n => n.TeacherId == teacherId).ToList();
        }

        public void Add(Note note)
        {
            note.Id = _nextId++;
            _notes.Add(note);
        }
        public Note GetById(int id)
        {
            //firstoedefault: a linq method that search the list and returns first element  that matches the condition or null if no element matches
            return _notes.FirstOrDefault(n => n.Id == id);
        }

        public void Update(Note note)
        {
            var existing = GetById(note.Id);
            if (existing != null)
            {
                existing.Title = note.Title;
                existing.FileName = note.FileName;
            }
        }

        public void Delete(int id)
        {
            var note = GetById(id);
            if (note != null)
                _notes.Remove(note);
        }
    }
}
