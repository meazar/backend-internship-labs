using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public abstract class Person
    {
        public String Name;
        public int Age;

        public Person(String name, int age)
        {
            Name = name;
            Age = age;
        }
        public abstract void PerformRole();

    }
}
