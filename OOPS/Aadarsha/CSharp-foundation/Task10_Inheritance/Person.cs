public class Person
{
    public string? Name { get; set; } // Auto-implemented property; get retrieves, set assigns; nullable type allows null
    public int? Age { get; set; } // Nullable int; allows undefined (null) age

    // Default (parameterless) constructor; called automatically when derived class doesn't explicitly invoke a base constructor
    public Person()
    {
        Console.WriteLine("Default Person constructor called");
    }

    // Parameterized constructor to initialize the Name property
    public Person(string name)
    {
        Name = name;
        Console.WriteLine("Person constructor called");
    }
    public void Introduce()
    {
        Console.WriteLine($"Hi, I'm {Name}, {Age} years old.");
    }

    public virtual void Speak() // virtual keyword allows overriding in derived classes
    {
        Console.WriteLine("Person is speaking.");
    }
}
