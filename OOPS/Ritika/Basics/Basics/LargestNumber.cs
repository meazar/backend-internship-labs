using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics
{
    internal class LargestNumber
    {
        public void LargestNumbers() 
        {
            Console.WriteLine("Enter three numbers:");
            int a = Convert.ToInt32(Console.ReadLine());
            int b = Convert.ToInt32(Console.ReadLine());
            int c = Convert.ToInt32(Console.ReadLine());

            if (a > b && a > c)
            { 
                Console.WriteLine($"{a} is largest than {b} and {c}"); 
            }

            else if(b > c && b > a)
            {
                Console.WriteLine($"{b} is largest than {a} and {c}");
            }
            else 
            {
                Console.WriteLine($"{c} is largest than {a} and {b}");
            }
        }
    }
}
