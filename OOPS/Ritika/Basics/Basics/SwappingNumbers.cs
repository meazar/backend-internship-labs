using System;

namespace Basics
{
    class SwappingNumbers
    {
        public void SwappingNumber()
        {
            int a, b;

            Console.Write("Enter first number (a): ");
            a = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter second number (b): ");
            b = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"\nBefore Swap: a = {a}, b = {b}");

            a = a + b;
            b = a - b;
            a = a - b;

            Console.WriteLine($"After Swap: a = {a}, b = {b}");
            Console.ReadLine(); // keeps console window open
        }
    }
}
