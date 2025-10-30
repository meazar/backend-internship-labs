using System;
class Program
{
    static void Main()
    {
        // When creating a Student object, the Student() constructor is invoked.
        // Before the Student constructor body runs, the base (Person) default constructor is automatically called first and executed.
        // And then the Student constructor body runs which ensures that the base class is properly initialized before the derived class adds its own initialization.
        Student student1 = new Student()
        {
            Name = "Aadarsha",
            Age = 20,
            StudentId = "THA077BEI001"
        };
        student1.Introduce(); // Inherited method from Person
        student1.Study(); // Defined in Student
        student1.Speak(); // Overridden method in Student

        Teacher teacher1 = new Teacher()  // Default constructor of Teacher is invoked → automatically calls Person() default constructor
        {
            Name = "Mr. Smith",
            Age = 30,
            Subject = "Mathematics"
        };
        teacher1.Introduce(); // Inherited from Person
        teacher1.Teach(); // Defined in Teacher

        // When creating a Student(name, id), the base Person(name) constructor executes first via base(name),
        // then the Student(name, id) constructor body runs.
        Student student2 = new Student("Aayush", "THA077BEI002");
        student2.Age = 22;
        student2.Introduce(); // Inherited from Person
        student2.Study(); // Defined in Student
    }
}
