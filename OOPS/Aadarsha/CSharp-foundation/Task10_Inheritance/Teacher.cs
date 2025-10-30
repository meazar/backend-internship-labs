using System;
public sealed class Teacher: Person  // sealed class cannot be inherited further
{
    public string? Subject { get; set; }

    // Default Constructor calling base class Person default constructor.
    public Teacher() : base() 
    {
        Console.WriteLine("Default Teacher constructor called");
    }
    public void Teach()
    {
        Console.WriteLine($"{Name} is teaching {Subject}.");
    }
}