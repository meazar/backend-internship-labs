using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopPractice
{
    internal class SumNaturalNumber
    {
        public void Sum() 
        {
            Console.WriteLine("Enter positive number");
            int num = Convert.ToInt32(Console.ReadLine());

            int sum = 0;

            for (int i = 1; i < num; i++)
            {

                sum += i;
            }
            Console.WriteLine($"The sun of the given natural number is {sum}");
            Console.ReadLine();

        }
    }
}
