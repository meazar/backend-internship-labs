using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuessingGame
{
    internal class GuessNumber
    {
        public void Guess()
        {
            Random random = new Random();
            int secretNumber = random.Next(1, 101); // generates number between 1 and 100
            int guess = 0;
            int attempts = 0;

            Console.WriteLine(" Welcome to the Number Guessing Game!");
            Console.WriteLine("I'm thinking of a number between 1 and 100.");
            Console.WriteLine("Try to guess it!\n");

            // Loop until user guesses correctly
            while (guess != secretNumber)
            {
                Console.Write("Enter your guess: ");
                guess = Convert.ToInt32(Console.ReadLine());
                attempts++;

                if (guess<secretNumber)
                {
                    Console.WriteLine("Too low! Try again.\n");
                }
                else if (guess > secretNumber)
                {
                    Console.WriteLine("Too high! Try again.\n");
                }
                else
                {
                    Console.WriteLine($"\n🎉 Correct! The number was {secretNumber}.");
                    Console.WriteLine($"You guessed it in {attempts} attempts.");
                }
            }

            Console.WriteLine("\nThanks for playing!");
            Console.ReadLine(); // keep console open
        }
    }
}
