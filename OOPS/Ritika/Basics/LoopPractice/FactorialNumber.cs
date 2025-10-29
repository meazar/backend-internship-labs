using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopPractice
{
    internal class FactorialNumber
    {
        public void FactorialNumbers()
        {
            Console.WriteLine("Enter positive number:");
            int num = Convert.ToInt32(Console.ReadLine());

            long result = 1;

            if (num < 0)
            {
                Console.WriteLine("There is no factorial of negative number");
            }
            else
            {
                for (int i = 1; i <= num; i++)
                {
                    result *= i;
                    
                }
                Console.WriteLine($"The factorial of {num} is: {result}");
            }
        }
    }
}
