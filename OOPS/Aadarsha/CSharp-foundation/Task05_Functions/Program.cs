using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("---Functions in C#---");

        Console.WriteLine("\n1. Function with Default Parameter Value:");
        Greet();
        Greet("Aadarsha");

        Console.WriteLine("\n2. Function overloading:");
        int sum1 = Add(5, 10);
        Console.WriteLine($"Sum: {sum1}");
        double sum2 = Add(5.5, 10.3);
        Console.WriteLine($"Sum: {sum2}");

        Console.WriteLine("\n3. Function with 'ref' Parameter:");
        int a = 7;
        Square(ref a); // Passing variable 'a' by reference
        Console.WriteLine($"Squared: {a}");

        Console.WriteLine("\n4. Function with 'out' Parameters:");
        Divide(20, 3, out int q, out int r);
        Console.WriteLine($"Quotient: {q}, Remainder: {r}");

        Console.WriteLine("\n");
        DisplayInfo(age: 22, name: "Suman");


    }

    static void Greet(string name = "Guest")
    {
        Console.WriteLine($"Hello, {name}!");
    }
    static int Add(int a, int b)
    {
        return a + b;
    }

    static double Add(double a, double b)
    {
        return a + b;
    }

    // ref parameter is used to pass a variable by reference so that any changes made to the parameter inside the function affect the original variable.
    static void Square(ref int number)
    {
        number = number * number;
    }

    /*
        4. Function with 'out' Parameters:
        Out parameters allow a function to return multiple values.
        The caller does not need to initialize the out parameters before passing them to the function.
    */
    static void Divide(int a, int b, out int quotient, out int remainder)
    {
        quotient = a / b;
        remainder = a % b;
    }

    static void DisplayInfo(string name, int age)
    {
        Console.WriteLine($"Name: {name}, Age: {age}");
    }
}