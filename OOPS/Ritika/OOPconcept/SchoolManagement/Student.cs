using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class Student : Person
    {
        private string Grade {  get; set; }

        public Student(string name, int age, string grade): base(name, age)
        {
            Grade = grade;
        }

        public void AttendClass()
        {
            Console.WriteLine($"{Name} (Grade: {Grade}) is attending class. {Name} is {Age} years old.");
        }

        public override void PerformRole()
        {
            AttendClass();
        }
    }
}
