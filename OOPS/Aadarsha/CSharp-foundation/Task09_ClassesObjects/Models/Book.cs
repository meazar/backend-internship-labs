namespace Task09_ClassesObjects.Models
{
    public class Book
    {
        public string? title;
        public string? author;
        public double? price;

        public void DisplayDetails()
        {
            Console.WriteLine($"Title: {title}, Author: {author}, Price: {price}");
        }
    }
}