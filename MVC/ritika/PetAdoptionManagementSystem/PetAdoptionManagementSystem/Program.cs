using System;
using Microsoft.Extensions.Configuration;
using PetAdoptionManagementSystem.Data;
using PetAdoptionManagementSystem.Models;
using PetAdoptionManagementSystem.Services;
using PetAdoptionManagementSystem.utils;

namespace PetAdoptionManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        //string password = "Admin123";
        //string hashed = PasswordHasher.Hash(password);
        //Console.WriteLine($"Hashed password: {hashed}");

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        DatabaseHelper db = new DatabaseHelper(config);
        AuthService authService = new AuthService(db);
        PetService petService = new PetService(db);
        AdoptionService adoptionService = new AdoptionService(db, petService);

        while (true)
        {
            Console.WriteLine("\n--------Pet Adoption System---------");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");
            Console.WriteLine("Select an Option");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Register(authService);
                    break;
                case "2":
                    var user = Login(authService);
                    if (user != null)
                    {
                        if (user.Role == UserRole.Admin)
                            AdminMenu(user, petService, adoptionService);
                        else
                            AdopterMenu(user, petService, adoptionService);

                    }
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option!");
                    break;

            }
        }

    }

    static void Register(AuthService authService)
    {
        Console.WriteLine("\n--- Register ---");
        User user = new User();

        Console.Write("Full Name: ");
        user.FullName = Console.ReadLine();

        Console.Write("Email: ");
        user.Email = Console.ReadLine();

        Console.Write("Password: ");
        user.Password = ReadPassword();

        Console.Write("Address: ");
        user.Address = Console.ReadLine();

        Console.Write("Phone: ");
        user.Phone = Console.ReadLine();

        user.Role = UserRole.Adopter;

        authService.Register(user);


        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();

    }

    static User Login(AuthService authService)
    {
        int maxAttempts = 3;
        int attempt = 0;
        User user = null;

        while (attempt < maxAttempts && user == null)
        {
            Console.WriteLine("\n---Login---");
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Password: ");
            string password = ReadPassword();

            user = authService.Login(email, password);
            if (user == null)
            {
                attempt++;
                Console.WriteLine($"Invalid email or password. Attempts left: {maxAttempts - attempt}");
            }

            
        }
        if (user == null)
        {
            Console.WriteLine("Too many failed attempts. Access denied.");
        }
        if (user != null)
        {
            Console.WriteLine($"Welcome {user.FullName} ({user.Role})");
        }
        return user;

    }

    static void AdminMenu(User admin, PetService petService, AdoptionService adoptionService)
    {
        while(true)
        {
            Console.WriteLine("\n--- Admin Menu ---");
            Console.WriteLine("1. Add Pet");
            Console.WriteLine("2. View All Pets");
            Console.WriteLine("3. View All Adoptions");
            Console.WriteLine("0. Logout");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddPet(petService);
                    break;
                case "2":
                    ViewPets(petService);
                    break;
                case "3":
                    ViewAllAdoptions(adoptionService);
                    break;

                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }
    }

    static void AdopterMenu(User adopter, PetService petService, AdoptionService adoptionService)
    {
        while (true)
        {
            Console.WriteLine("\n--- Adopter Menu ---");
            Console.WriteLine("1. View Available Pets");
            Console.WriteLine("2. Adopt a Pet");
            Console.WriteLine("3. View My Adoptions");
            Console.WriteLine("0. Logout");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewPets(petService);
                    break;

                case "2":
                    AdoptPet(adopter, petService, adoptionService);
                    break;
                case "3":
                    ViewMyAdoption(adopter, adoptionService);
                    break;
                case "0": 
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    static void AddPet(PetService petService)
    {
        Console.WriteLine("\n--- Add New Pet ---");
        Pet pet = new Pet();

        Console.Write("Name: ");
        pet.Name = Console.ReadLine();

        Console.Write("Species: ");
        pet.Species = Console.ReadLine();

        Console.Write("Breed: ");
        pet.Breed = Console.ReadLine();

        Console.Write("Age: ");
        pet.Age = int.Parse(Console.ReadLine());

        Console.Write("Gender (Male/Female): ");
        pet.Gender = Console.ReadLine();

        petService.AddPet(pet);

    }

    static void ViewPets (PetService petService)
    {
        var pets = petService.GetAvailablePets();
        Console.WriteLine("\n--- Available Pets ---");
        foreach (var pet in pets)
        {
            Console.WriteLine(pet);
        }
    }
    static void AdoptPet(User adopter, PetService petService, AdoptionService adoptionService)
    {
        ViewPets(petService);
        Console.Write("Enter Pet Id to adopt: ");
        int PetId = int.Parse(Console.ReadLine());
        adoptionService.AdoptPet(PetId, adopter.UserId);
    }
    static void ViewMyAdoption(User adopter, AdoptionService adoptionService)
    {
        var adoptions = adoptionService.GetAdoptionsByUser(adopter.UserId);
        Console.WriteLine("\n --- My Adoption ---");
        foreach (var a in adoptions)
        {
            Console.WriteLine(a);
        }

    }
    static void ViewAllAdoptions(AdoptionService adoptionService)
    {
        var adoptions = adoptionService.GetAllAdoptions();
        Console.WriteLine("\n --- All Adoption ---");

        if(adoptions.Count == 0)
        {
            Console.WriteLine("No adoption yet");
            return;
        }

        foreach (var a in adoptions)
        {
            Console.WriteLine(a);
        }

    }

    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(intercept: true);//prevents key frombeing displayed
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];//takes all character from index 0 up to last character 
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

}