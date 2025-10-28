using System;

namespace Task04_ControlFlow
{
    class Program
    {
        static void Main()
        {
            // if / else if / else
            Console.WriteLine("If / Else If / Else:");
            int number = 7;
            if (number > 0)
            {
                Console.WriteLine($"{number} is positive.");
            }
            else if (number < 0)
            {
                Console.WriteLine($"{number} is negative.");
            }
            else
            {
                Console.WriteLine("The number is zero.");
            }

            // Nested if
            Console.WriteLine("\nNested If:");
            int age = 20;
            if (age >= 18)
            {
                if (age >= 21)
                {
                    Console.WriteLine("You are eligible to vote and drink alcohol.");
                }
                else
                {
                    Console.WriteLine("You are eligible to vote but not to drink alcohol.");
                }
            }
            else
            {
                Console.WriteLine("You are not eligible to vote.");
            }

            // ternary operator 
            Console.WriteLine("\nTernary Operator:");
            int score = 85;
            string result = (score >= 50) ? "Passed" : "Failed";
            Console.WriteLine($"Score: {score} - {result}");

            // switch statement
            Console.WriteLine("\nSwitch Statement:");
            int dayNumber = 3;

            string dayName = dayNumber switch
            {
                1 => "Sunday",
                2 => "Monday",
                3 => "Tuesday",
                4 => "Wednesday",
                5 => "Thursday",
                6 => "Friday",
                7 => "Saturday",
                _ => "Invalid Day" // '_' acts as the default case
            };
            Console.WriteLine(dayName);

            // for loop
            Console.WriteLine("\nFor loop:");
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Iteration - {i}");
            }

            // while loop
            Console.WriteLine("\nWhile loop:");
            int count = 1;
            while (count <= 3)
            {
                Console.WriteLine($"Count: {count}");
                count++;
            }

            // do...while loop
            Console.WriteLine("\ndo...while loop");
            int num = 0;
            do
            {
                Console.WriteLine($"Count: {num}");
                num++;
            } while (num < 3);

            // foreach loop
            Console.WriteLine("\nForeach loop:");
            string[] fruits = { "Apple", "Banana", "Cherry" };
            foreach(string fruit in fruits)
            {
                Console.WriteLine(fruit);
            }
        }
    }
}