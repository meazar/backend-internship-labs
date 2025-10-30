using System;
namespace LibraryManagementSystem;

class Program
{
    public static void Main (string[] args)
    {
        Member member = new Member("Ritika");
        Librarian librarian = new Librarian("Mastermind");

        member.DisplayRole();
        librarian.DisplayRole();

        Ebook ebook = new Ebook("Harry Potter", "JK Rowling", 5.4);
        PrintedBook pb = new PrintedBook("OOP Basics", "Alaska Goddess", 300);

        ebook.DisplayInfo();
        pb.DisplayInfo();
    }
}