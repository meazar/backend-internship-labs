using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps
{
    internal class CalculatorApp
    {
        public void Calculator() 
        {
            Console.WriteLine("Enter first number");
            double num1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter second number");
            double num2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Choose the following operation");
            Console.WriteLine("1. Add'+'");
            Console.WriteLine("2. Subtract'-'");
            Console.WriteLine("3. Multiply'*'");
            Console.WriteLine("4. Divide'%'");


            char op = Console.ReadLine()[0];

            double result = 0;

            switch (op)
            {
                case '+':
                    result = num1 + num2;
                    break;

                case '-':
                    result = num1 - num2;
                    break;

                case '*':
                    result = num1 * num2;
                    break;

                case '%':
                    if(num2 != 0)
                    {
                        result = num1 % num2;
                        
                    }else
                    {
                        Console.WriteLine("Enter Positive Number");
                    }
                    break;

                default:
                    Console.WriteLine("Choose the correct operation");
                    break;
            }

            Console.WriteLine($"\nResult: {num1} {op} {num2} = {result}");
            Console.ReadLine();



        }
    }
}
