namespace Task09_ClassesObjects.Models
{
    class Student
    {
        public string? name;
        public int? rollNumber;

        // constructor is a special method that is called when an object is instantiated
        public Student(string studentName, int studentRollNumber)
        {
            name = studentName;
            rollNumber = studentRollNumber;
        }   
        public void DisplayInfo()
        {
            Console.WriteLine($"Name: {name}, Roll Number: {rollNumber}");
        }
    }
}