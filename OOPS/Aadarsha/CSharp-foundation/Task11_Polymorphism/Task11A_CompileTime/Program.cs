/* Compile-Time Polymorphism (Static Polymorphism) 
    
    This type of polymorphism is resolved by the C# compiler at compile time based on the method signature 
    (name, number, and type of parameters).

    A) Method Overloading:
        - Achieved through method overloading (same method name, different parameters)
        - Resolved during compile time (Compiler knows which method to call based on arguments provided)
        - Example: Multiple methods with the same name but different signatures
        
    B) Operator Overloading:
        - Achieved through operator overloading (customizing the behavior of operators for user-defined types)
        - Resolved during compile time (Compiler knows which custom operator method to call)
        - Example: Overloading the + operator for a custom class
*/

//  Method Overloading:
class Calculator
{
    // Overload 1: Adds two integers. Signature: Add(int, int)
    public int Add(int x, int y) => x + y;
    
    // Overload 2: Adds two doubles. Different parameter types. Signature: Add(double, double)
    public double Add(double x, double y) => x + y;
    
    // Overload 3: Adds three integers. Different number of parameters. Signature: Add(int, int, int)
    public int Add(int x, int y, int z) => x + y + z;
}
class MethodOverloadingDemo
{
    public static void Run()
    {
        Console.WriteLine("\nMethod Overloading Demo:\n");
        Calculator cal1 = new Calculator();
        
        // Compile-Time Resolution: Compiler chooses Add(int, int)
        Console.WriteLine(cal1.Add(2, 3));          
        
        // Compile-Time Resolution: Compiler chooses Add(double, double)
        Console.WriteLine(cal1.Add(2.33, 3.33));    
        
        // Compile-Time Resolution: Compiler chooses Add(int, int, int)
        Console.WriteLine(cal1.Add(1, 2, 3));       
    }

}

// Operator Overloading
class Complex
{
    public double Real { get; set; }
    public double Imag { get; set; }
    
    // Constructor to initialize the complex number
    public Complex(double real, double imag)
    {
        Real = real;
        Imag = imag;
    }

    // Overload '+' operator
    // The 'operator' keyword is used to declare an operator method. It tells the compiler that the following method defines the behavior of an operator (in this case, '+' and '-').
    // The method must be declared 'public' and 'static'. It must be 'static' because operator overloading is essentially a static function call
    // that operates on its operands (c1 and c2), not on a specific instance of the Complex class.
    public static Complex operator +(Complex c1, Complex c2)
    {
        // 'new' here creates and returns a NEW Complex object as the result of the addition.
        return new Complex(c1.Real + c2.Real, c1.Imag + c2.Imag);
    }

    // Overload '-' operator
    public static Complex operator -(Complex c1, Complex c2)
    {
        return new Complex(c1.Real - c2.Real, c2.Imag - c2.Imag);
    }

    // This method is overridden from the base 'object' class.
    // It is used here to provide a custom, readable string representation of the Complex object.
    // Without this override, Console.WriteLine() would just print the object's type name.
     public override string ToString() => $"{Real} + {Imag}i";
}

class OperatorOverloadingDemo
{
    public static void Run()
    {
        Console.WriteLine("\nOperator Overloading Demo (complex number):\n");
        Complex num1 = new Complex(3, 5);
        Complex num2 = new Complex(4, 6);

        // This line calls the custom 'public static Complex operator +(Complex c1, Complex c2)' method.
        Complex sum = num1 + num2; 
        // This line calls the custom 'public static Complex operator -(Complex c1, Complex c2)' method.
        Complex diff = num2 - num1;

        Console.WriteLine($"num1 = {num1}"); // Calls Complex.ToString()
        Console.WriteLine($"num2 = {num2}"); // Calls Complex.ToString()
        Console.WriteLine($"Sum = {sum}");   
        Console.WriteLine($"Difference = {diff}");

    }
}

class Program
{
    static void Main()
    {
        // run method overloading demo
        MethodOverloadingDemo.Run();

        // run operator overloading demo
        OperatorOverloadingDemo.Run();

    }
}