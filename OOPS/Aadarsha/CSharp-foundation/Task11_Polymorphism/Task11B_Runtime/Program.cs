/*
    Runtime polymorphisim:
    Runtime polymorphism is a process in which a call to an overridden method is resolved at runtime rather than compile-time.
    This is typically achieved through method overriding, where a subclass provides a specific implementation of a method that is already defined in its superclass.
    In C#, runtime polymorphism is implemented using virtual methods and method overriding.
    When a method is declared as virtual in a base class, it can be overridden in any derived class using the override keyword.
    When a method is called on a base class reference that points to a derived class object, the overridden method in the derived class is executed.
    This allows for dynamic method resolution, enabling more flexible and extensible code.
    - Achieved using inheritance + virtual methods + override.
    - Base class defines a virtual method, derived class overrides it.
    - Method to be called is determined at runtime based on the object type.
*/

// Virtual and Override 
using System;

// Base class defining a method as virtual for potential overriding
class Person
{
    public virtual void ShowInfo() // virtual keyword allows derived classes to override this method
    {
        Console.WriteLine("I am a person.");
    }
}

// Derived class inheriting from Person
class Student : Person
{
    public override void ShowInfo() // override keyword provides a specific implementation for the virtual method in the base class
    {
        Console.WriteLine("I am a student.");
    }
}

// another Derived class inheriting from Person
class Teacher : Person
{
    public override void ShowInfo() 
    {
        Console.WriteLine("I am a teacher.");
    }
}

class Program
{
    static void Main()
    {
        Person p1 = new Person(); // p1 is a Person reference pointing to a Person object. Calls Person's ShowInfo.
        Person p2 = new Student(); // p2 is a Person reference pointing to a Student object (Runtime Polymorphism). Calls Student's ShowInfo.
        Person p3 = new Teacher(); // p3 is a Person reference pointing to a Teacher object (Runtime Polymorphism). Calls Teacher's ShowInfo.

        // The method called is determined by the actual object type at runtime
        p1.ShowInfo();
        p2.ShowInfo();
        p3.ShowInfo();
    }
}