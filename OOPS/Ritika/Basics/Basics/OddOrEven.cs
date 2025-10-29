using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics
{
    internal class OddOrEven
    {
        public void OddEven() 
        {
           
            Console.WriteLine("Enter any positive number: ");
            int a = Convert.ToInt32(Console.ReadLine());

            if (a%2 == 0)
            {
                Console.WriteLine("It is an even number");

            }
            else
            {
                Console.WriteLine("It is an odd number");
            }
        }
    }
}
