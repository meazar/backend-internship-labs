using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDetails
{
    internal class StudentDetail
    {
        public void Student()
        {

            Console.WriteLine("--- Enter Student Details ---");

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("GPA: ");
            double gpa = Convert.ToDouble(Console.ReadLine());

            Console.Write("Grade (A/B/C/D/F): ");
            char grade = Convert.ToChar(Console.ReadLine().ToUpper());

            Console.Write("Enrolled (true/false): ");
            bool isEnrolled = Convert.ToBoolean(Console.ReadLine());


            Console.WriteLine("\n--- Student Details ---");
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Age: " + age);
            Console.WriteLine("GPA: " + gpa);
            Console.WriteLine("Grade: " + grade);
            Console.WriteLine("Enrolled: " + isEnrolled);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();

        }
    }
}
