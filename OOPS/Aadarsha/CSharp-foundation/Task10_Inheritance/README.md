## Constructor chaining:
 is a technique where a constructor calls another constructor in the same class or the base class to reuse code and ensure proper initialization of objects.
In inheritance, when a derived class constructor is called, it can invoke a base class constructor using the 'base' keyword.
This ensures that the base class is properly initialized before the derived class adds its own initialization.
In C#, if a derived class constructor does not explicitly call a base class constructor, the default (parameterless) constructor of the base class is called automatically.
In the provided code, the Student and Teacher classes inherit from the Person class.
The Student class demonstrates constructor chaining by explicitly calling the base class constructors using the 'base' keyword in both its default and parameterized constructors.
The Teacher class also calls the base class default constructor explicitly in its default constructor.
This ensures that the Person class is properly initialized before any additional initialization in the Student or Teacher classes takes place. 
## Task 10: Inheritance and Constructor Chaining in C#
This task demonstrates the concept of inheritance and constructor chaining in C# using a simple example with a base class `Person` and two derived classes `Student` and `Teacher`.
### Classes Overview
1. **Person Class**:
   - Properties: `Name` (string), `Age` (int)
   - Constructors:
     - Default constructor: Initializes a `Person` object and prints a message.
     - Parameterized constructor: Initializes the `Name` property and prints a message.
   - Method: `Introduce()` - Prints a greeting message with the person's name and age.
2. **Student Class** (inherits from `Person`):
   - Property: `StudentId` (string)
   - Constructors:
     - Default constructor: Calls the base class default constructor and prints a message from Default constructor Person at first and then of constructor Student().
     - Parameterized constructor: Calls the base class parameterized constructor to initialize `Name` and then it initializes `StudentId` in it's own, and prints a message.
   - Method: `Study()` - Prints a message indicating that the student is studying.
3. **Teacher Class** (inherits from `Person`):
   - Property: `Subject` (string)
   - Constructor:
     - Default constructor: Calls the base class default constructor and prints a message.
   - Method: `Teach()` - Prints a message indicating that the teacher is teaching a subject.
### Program Execution
- The `Main` method in the `Program` class demonstrates the creation of `Student` and `Teacher` objects.
- It shows how the constructors are called in sequence, starting from the base class to the derived class, ensuring proper initialization.
- The program also calls the `Introduce()`, `Study()`, and `Teach()` methods to demonstrate the functionality of each class.
### Key Concepts
- **Inheritance**: The `Student` and `Teacher` classes inherit from the `Person` class, allowing them to reuse code and properties defined in the base class.
- **Constructor Chaining**: The use of the `base` keyword in the constructors of the derived classes to call the appropriate base class constructors, ensuring that the base class is initialized before the derived class adds its own initialization.
- **Sealed Class**: The `Teacher` class is marked as `sealed`, indicating that it cannot be further inherited.  This is useful for preventing further derivation and ensuring the integrity of the class design. (Prevents a class from being) inherited.
- **protected**: Members accessible only in the class itself or derived classes.



### Destructors
A destructor is a special method that is automatically called by the garbage collector (GC) before an object is destroyed (freed from memory).
It is used to release unmanaged resources (like file handles, database connections, native memory, etc.) that the .NET GC cannot clean up automatically.


