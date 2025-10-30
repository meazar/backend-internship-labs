using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class QuizApp
    {
        public void QuizApps()
        {
            string[] questions = {
                "What is the capital of France?",
                "Which planet is known as the Red Planet?",
                "What is 5 + 7?"
            };

            string[,] options = {
                { "1. London", "2. Berlin", "3. Paris", "4. Madrid" },
                { "1. Earth", "2. Mars", "3. Jupiter", "4. Venus" },
                { "1. 10", "2. 12", "3. 11", "4. 15" }
            };

            int[] answers = { 3, 2, 2 }; // correct option numbers
            int score = 0;

            Console.WriteLine("🎓 Welcome to the Simple Quiz App!\n");

            for (int i = 0; i < questions.Length; i++)
            {
                Console.WriteLine(questions[i]);
                for (int j = 0; j < 4; j++)
                {
                    Console.WriteLine(options[i, j]);
                }

                Console.Write("Your answer (1-4): ");
                int userAnswer = Convert.ToInt32(Console.ReadLine());

                if (userAnswer == answers[i])
                {
                    Console.WriteLine("Correct!\n");
                    score++;
                }
                else
                {
                    Console.WriteLine("Wrong!\n");
                }
            }

            Console.WriteLine($"Quiz Over! Your score: {score}/{questions.Length}");
            Console.ReadLine();

        }
    }
}
