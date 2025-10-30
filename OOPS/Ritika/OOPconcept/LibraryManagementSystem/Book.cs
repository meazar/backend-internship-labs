using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Book
    {
        private string title;
        private string author;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        protected Book(string title, string author)
        {
            this.title = title;
            this.Author = author;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Book: {Title} by {Author}");
        }
    }
}
