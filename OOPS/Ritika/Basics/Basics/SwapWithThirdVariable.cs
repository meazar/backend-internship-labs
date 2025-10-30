using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics
{
    internal class SwapWithThirdVariable
    {
        public void SwapWithThirdVariables()
        {
                int a, b, temp;

                Console.Write("Enter first number (a): ");
                a = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter second number (b): ");
                b = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine($"\nBefore Swap: a = {a}, b = {b}");

                // Swap using third variable
                temp = a;
                a = b;
                b = temp;

                Console.WriteLine($"After Swap: a = {a}, b = {b}");
                Console.ReadLine(); 
            }
}
}

