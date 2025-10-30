using System.Drawing;
using System.Net;
using Task09_ClassesObjects.Models;

namespace Task09_ClassesObjects
{
    class Program
    {
        static void Main()
        {
            // Create an instance of the Car class
            Car myCar = new Car();
            myCar.brand = "Tesla";
            myCar.model = "Model Y";
            myCar.year = 2022;
            myCar.DisplayInfo();

            // Create an instance of the Person class
            // basic object creation without constructor
            Person person = new Person();
            person.firstName = "Sara";
            person.age = 18;
            person.Introduce();

            // Create an instance of the Student class with constructor
            Student s1 = new Student("Divya", 101);
            s1.DisplayInfo();
            Student s2 = new Student("Amit", 102);
            s2.DisplayInfo();

            // Create an instance of the BankAccount class
            BankAccount account = new BankAccount();
            account.Balance = 5000; // using setter to set balance
            account.ShowBalance(); // using method to show balance
            account.Balance = -100; // trying to set invalid balance
            account.ShowBalance();  // balance remains unchanged and shows the previous valid balance

            // Create an instance of the Book class by using object initializer syntax
            Book b1 = new Book { title = "C# Programming", author = "Alice", price = 399.99 };
            b1.DisplayDetails();

            // Create an instance of the Rectangle class use constructor to set length and breadth
            RectangleShape r1 = new RectangleShape(10, 5);
            Console.WriteLine($"Area: {r1.CalculateArea()}");
            Console.WriteLine($"Perimeter: {r1.CalculatePerimeter()}");

            // StudentWithValidation instance
            StudentWithValidation student1 = new StudentWithValidation();
            student1.Name = "Ram Khadka";
            student1.Marks = 85;
            Console.WriteLine($"Student Name: {student1.Name}, Marks: {student1.Marks}");
            student1.ShowResult();

            // account class instance
            Account acc1 = new Account("Rishav KC", "ACC123", 1000.99);
            acc1.DisplayBalance();
            acc1.Deposit(500);
            acc1.Withdraw(200);
            acc1.DisplayBalance();
            acc1.Withdraw(2000); // trying to withdraw more than balance
            acc1.DisplayBalance();
        }
    }       
}