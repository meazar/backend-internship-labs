using System;
using System.Globalization;
using VoterRegistrationSystem.Interfaces;
using VoterRegistrationSystem.Models;
using VoterRegistrationSystem.Services;

namespace VoterRegistrationSystem;

class Program
{
    static void Main(string[] args)
    {
        var voterService = new VoterService();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("-------Voter Registration System----------");
            Console.WriteLine("1. Register voter");
            Console.WriteLine("2. List All Voters");
            Console.WriteLine("3. Search by Citizenship ID");
            Console.WriteLine("4. Exit");
            Console.WriteLine("Choose an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch(choice)
            {
                case "1":
                    RegisterVoter(voterService);
                    break;
                case "2":
                    ListAllVoters(voterService);
                    break;
                case "3":
                    SearchVoter(voterService);
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid Choice.Try again.");
                    break;

            }
        }
    }
    static void RegisterVoter(VoterService service)
    {
        var voter = new Voter();

        Console.WriteLine("Enter Name: ");
        voter.Name = Console.ReadLine();

        Console.WriteLine("Enter Age: ");
        voter.Age = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter Citizenship Id:");
        voter.CitizenshipId = Console.ReadLine();

        Console.WriteLine("Enter Address: ");
        voter.Address = Console.ReadLine();

        service.RegisterVoter(voter);
    }

    static void ListAllVoters(VoterService service)
    {
        var voters = service.GetAllVoters();
        if(voters.Count == 0)
        {
            Console.WriteLine("No voters registered yet.");
            return;
        }

        foreach(var voter in voters)
        {
            voter.DisplayInfo();
        }
    }

    static void SearchVoter(VoterService service)
    {
        Console.WriteLine("Enter citizenship ID to search: ");
        string id = Console.ReadLine();

        var voter = service.SearchByCitizenship(id);
        if(voter == null)
        {
            Console.WriteLine("Voter not found.");
        }
        else
        {
            voter.DisplayInfo();
        }
    }
}