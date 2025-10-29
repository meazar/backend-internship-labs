using System;
using RecursionMethod; 
namespace Basics;
class Program
{
    static void Main(string[] args)
    {
        Factorial fc = new Factorial();
        Console.Write("Enter a non-negative integer: ");
        int number = Convert.ToInt32(Console.ReadLine());

        if (number < 0)
        {
            Console.WriteLine("Factorial is not defined for negative numbers.");
        }
        else
        {
            long result = fc.Factor(number); 
            Console.WriteLine($"Factorial of {number} is: {result}");
        }

        Console.ReadLine();
    }
}