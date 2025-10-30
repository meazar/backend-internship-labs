/* 
Compile-Time Polymorphism(Static Polymorphism)
    A) Method Overloading:
        - Achieved through method overloading (same method name, different parameters)
        - Resolved during compile time
        - Example: Multiple methods with the same name but different signatures
    B) Operator Overloading:
        - Achieved through operator overloading (customizing the behavior of operators for user-defined types)
        - Resolved during compile time
        - Example: Overloading the + operator for a custom class
*/


// Example of Method Overloading:
class Calculator
{
    public int Add(int x, int y) => x + y;
    public double Add(double x, double y) => x + y;
    public int Add(int x, int y, int z) => x + y + z;
}

class Program
{
    static void Main()
    {
        Calculator cal1 = new Calculator();
        Console.WriteLine(cal1.Add(2, 3));          // calls Add(int, int)
        Console.WriteLine(cal1.Add(2.33, 3.33));    // calls Add(double, double)
        Console.WriteLine(cal1.Add(1, 2, 3));       // calls Add(int, int, int)
    }
}