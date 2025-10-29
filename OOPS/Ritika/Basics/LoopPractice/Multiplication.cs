using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopPractice
{
    internal class Multiplication
    {
        public void Multiplications() 
        {
            Console.WriteLine("Enter any number");
            int num = Convert.ToInt32(Console.ReadLine());

            

            Console.WriteLine($"Multiplication of {num} is: ");

            for (int i = 1; i <= 10; i++) 
            {
                int result = num * i;
                Console.WriteLine($"{num} * {i} = {result}");
            }
                
        }
    }
}
