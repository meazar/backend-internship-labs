using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Ebook : Book
    {
        public double FileSizeMB{get;set;}

        public Ebook(string title, string author, double fileSize) :
            base(title, author)
        {
            FileSizeMB = fileSize;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Book: {Title} by {Author} ({FileSizeMB}MB)");
        }
    }
    public class PrintedBook : Book 
    {
        public int Pages { get;set;}

        public PrintedBook(string title, string author, int pages) : 
            base(title, author) 
        {
            Pages = pages;
        }
        public override void DisplayInfo() 
        {
            Console.WriteLine($"Printed Book: {Title} by {Author}, {Pages} pages");
        }
    }
}
