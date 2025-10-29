using System;
namespace SchoolManagement;

class Program
{
    public static void Main (string[] args)
    {
        Student std = new Student("Mysterious",12,"A");
        Teacher tch = new Teacher("Strange",25,"BioTechonology");

        std.PerformRole();
        tch.PerformRole();
    }

}