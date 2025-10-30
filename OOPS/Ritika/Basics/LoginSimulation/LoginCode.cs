using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSimulation
{
    public class LoginCode
    {
        public void Login() 
        {
            string correctUsername = "ritika";
            string correctPassword = "ritika123";

            int maxAttempt = 3;
            int attempt = 0;
            bool isLoggedIn=false;

            while(attempt<maxAttempt && !isLoggedIn)
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                if (username == correctUsername && password == correctPassword)
                {
                    Console.WriteLine("\nLogin successful!Welcome," + username + "!");
                    isLoggedIn = true;
                }
                else
                {
                    attempt++;
                    Console.WriteLine("\nInvalid username or password.Attempts left: " + (maxAttempt - attempt));
                }
            }

            if (!isLoggedIn)
            {
                Console.WriteLine("\nToo many failed attempts.Access denied.");
            }

            Console.ReadLine();
        }
    }
}
