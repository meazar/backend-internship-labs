using System;

namespace Task07_Strings

{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("\n---String Fundamentals in C#---\n");

            // string declaration
            string name = "Aadarsha";
            string language = "C#";
            Console.WriteLine($"Hello, {name}! Welcome to {language} programming.");

            // string concatenation
            string greet = "Hello, " + name + "!" + " Let's learn " + language + ".";
            Console.WriteLine(greet);

            // string interpolation
            string interpolatedGreet = $"Hello, {name}! Let's learn {language} together.";
            Console.WriteLine(interpolatedGreet);

            // string Length
            Console.WriteLine($"\nThe length of your name is: {name.Length} characters.");

            // accessing characters in a string
            Console.WriteLine($"First character of your name: {name[0]}, Last character: {name[name.Length - 1]}");

            // escape sequences, characters like \n, \t, \", \\
            Console.WriteLine("This is tabbed text:\tafter tab.");
            Console.WriteLine("This is a new line:\nNewline");
            string path = "C:\\Users\\Aadarsha\\Documents\\CSharp";
            Console.WriteLine($"Your file is located at: {path}");

            // verbatim string literal is prefixed with @ that ignores escape sequences
            string verbatimPath = @"C:\Users\Aadarsha\Documents\CSharp";
            Console.WriteLine($"Verbatim string path: {verbatimPath}");


            Console.WriteLine("\nCommon String Methods:\n");
            string sentence = " A quick brown fox jumps over the lazy dog. ";
            Console.WriteLine($"Original Sentence: '{sentence}'");

            // trim() method to remove leading and trailing whitespace
            string trimmedSentence = sentence.Trim();
            Console.WriteLine($"Trimmed Sentence: '{trimmedSentence}'");

            // toUpper() method to convert string to uppercase
            string upperSentence = trimmedSentence.ToUpper();
            Console.WriteLine($"Uppercase Sentence: '{upperSentence}'");

            // toLower() method to convert string to lowercase
            string lowerSentence = trimmedSentence.ToLower();
            Console.WriteLine($"Lowercase Sentence: '{lowerSentence}'");

            // Contains() method to check if a substring exists
            bool containsFox = trimmedSentence.Contains("fox");
            Console.WriteLine($"Contains 'fox': {containsFox}");

            // StartsWith() method to check starting substring
            bool startsWithA = trimmedSentence.StartsWith("A");
            Console.WriteLine($"Starts with 'A': {startsWithA}");

            // EndsWith() method to check ending substring
            bool endsWithDog = trimmedSentence.EndsWith("dog.");
            Console.WriteLine($"Ends with 'dog.': {endsWithDog}");

            // Replace() method to replace a substring with another substring
            string replacedSentence = trimmedSentence.Replace("fox", "cat");
            Console.WriteLine($"Replaced Sentence: '{replacedSentence}'");

            // Substring() method to extract a part of the string
            string substring = trimmedSentence.Substring(8, 5); // "brown"
            Console.WriteLine($"Extracted Substring: '{substring}'");

            // IndexOf() method to find the index of a substring of first occurrence.
            int indexOfJumps = trimmedSentence.IndexOf("jumps");
            Console.WriteLine($"Index of 'jumps': {indexOfJumps}");


            // Strings in C# are immutable — once created, they cannot be changed in memory. Any modification creates a new string.
            Console.WriteLine("\n ---String immutability in C#---\n");
            string Original = "DevOps";
            string Modified = Original.Replace("Dev", "Cloud");
            Console.WriteLine($"Original String: {Original}"); // DevOps
            Console.WriteLine($"Modified String: {Modified}"); // CloudOps
        }
    }
}