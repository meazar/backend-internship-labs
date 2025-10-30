namespace Task09_ClassesObjects.Models
{
    public class StudentWithValidation
    {
        private string? name;
        private double? marks;
        public string Name
        {
            get { return name; } // Gettter returns the value of name field
            set { name = value; } // No specific validation for name, name gets assigned directly as provided
        }
        public double Marks
        {
            get { return (double)marks; } // Getter returns the value of marks field
            set
            {
                if (value >= 0 && value <= 100)
                    marks = value;
                else
                    Console.WriteLine("Invalid Marks! allowed(0-100).");
            }
        }
        public void ShowResult()
        {
            string grade;
            if (marks >= 90) grade = "A";
            else if (marks >= 80) grade = "B";
            else if (marks >= 70) grade = "C";
            else if (marks >= 60) grade = "D";
            else grade = "Fail";
            Console.WriteLine($"Student: {name}, Marks: {marks}, Grade: {grade}");
        }
    }
}