namespace Task09_ClassesObjects.Models;

class Person
{
    public string? firstName;
    public int? age;

    public void Introduce()
    {
        Console.WriteLine($"Hi, I'm {firstName} and I'm {age} years old.");
    }

}