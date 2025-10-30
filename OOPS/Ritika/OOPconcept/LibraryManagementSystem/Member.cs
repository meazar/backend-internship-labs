using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Member :Person
    {
        public Member(string name) : base(name) { }

        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} is Library Member");
        }
    }

    public class Librarian : Person
    {
        public Librarian(string name) : base(name) { }

        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} is the Librarian");
        }

    }
}
