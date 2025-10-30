/*
    Polymorphism with Abstract Classes and Interfaces:
    Abstract classes can define abstract methods (no body), forcing derived classes to implement them.
    Interfaces allow implementing polymorphism across unrelated classes.

    The example below demonstrates interface-based polymorphism.
*/

// Interface declaration: Defines a contract (a set of methods/properties)
interface IAnimal
{
    // Interface method: Does not contain an implementation (no body).
    // All classes that implement IAnimal must provide a body for this method.
    void Speak();
}

// Class Dog implements the IAnimal interface.
class Dog : IAnimal
{
    // The 'Speak' method from the interface is explicitly implemented.
    public void Speak() => Console.WriteLine("Woof!");
}

// Class Cat implements the IAnimal interface, demonstrating polymorphism.
class Cat : IAnimal
{
    // Cat provides its own unique implementation for the 'Speak' method.
    public void Speak() => Console.WriteLine("Meow!");
}

class Program
{
    static void Main()
    {
        IAnimal a1 = new Dog(); // Interface reference pointing to a Dog object.
        IAnimal a2 = new Cat(); // Interface reference pointing to a Cat object.

        // Polymorphism at work: The same 'Speak()' call executes different code
        // based on the actual object type (Dog or Cat) at runtime.
        a1.Speak();
        a2.Speak(); 
    }
}