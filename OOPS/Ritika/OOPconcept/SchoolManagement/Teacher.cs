using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public  class Teacher: Person
    {
        public string Subject {  get; set; }
        public Teacher(string name, int age , string subject ) : base(name, age)
        {
            Subject = subject;
        }
        public void TeachSubject()
        {
            Console.WriteLine($"{Name} teaches {Subject}.{Name} is {Age} years old.");
        }
        public override void PerformRole()
        {
            TeachSubject();
        }
    }
}
