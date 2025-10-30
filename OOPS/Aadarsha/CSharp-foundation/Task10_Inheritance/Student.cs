using System;
public class Student : Person
{
    public string? StudentId { get; set; }

    // Default Constructor calling base class Person default constructor.
    public Student() : base() 
    {
        Console.WriteLine("Default Student constructor called");
    }

    // Student Constructor calling base class Person constructor
    public Student(string name, string id) : base(name)
    {
        StudentId = id;
        Console.WriteLine("Student parameterized constructor called");
    }
    public void Study()
    {
        Console.WriteLine($"{Name} is studying with Student ID: {StudentId}");
    }

    public override void Speak() // override keyword allows overriding the Speak method from Person class
    {
        base.Speak(); // Call the base class Speak method first 
        Console.WriteLine("Student is speaking.");
        
    }
}