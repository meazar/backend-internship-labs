namespace VehicleRentalAndServices
{
    using Service.IService;
    using System.ComponentModel;
    using static VehicleRentalAndServices.Vehicle;

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Vehicle> vehicles = new List<Vehicle>
            {
               new Car(1,"Toyota", "Camry", 50),
                new Bike(2,"Yamaha", "YZF-R3", 30),
                new Scooter(3,"Vespa", "Primavera", 20),


            };

            RentedService rentalService = new RentedService(vehicles);

            while (true)
            {
                Console.WriteLine("\nVehicle Rental and Service System");
                Console.WriteLine("1. Display Available Vehicles");
                Console.WriteLine("2. Display Vehicles Under Service");
                Console.WriteLine("3. Rent a Vehicle");
                Console.WriteLine("4. Return a Vehicle");
                Console.WriteLine("5. Send Vehicle for Service");
                Console.WriteLine("6. Complete Vehicle Service");
                Console.WriteLine("7. Exit");

                try
                {
                    int Choice = Convert.ToInt32(Console.ReadLine());

                    switch (Choice)
                    {
                        case 1:
                            rentalService.DisplayAvailableVehicles();
                            break;
                        case 2:
                            rentalService.DisplayserviceVehicles();
                            break;
                        case 3:
                            Console.Write("Enter Vehicle ID to Rent: ");
                            int rentId = Convert.ToInt32(Console.ReadLine());
                            rentalService.RentVehicle(rentId);
                            break;
                        case 4:
                            Console.Write("Enter Vehicle ID to Return: ");
                            int returnId = Convert.ToInt32(Console.ReadLine());
                            rentalService.ReturnVehicle(returnId);
                            break;
                        case 5:
                            Console.Write("Enter Vehicle ID to Send for Service: ");
                            int serviceId = Convert.ToInt32(Console.ReadLine());
                            rentalService.SendForService(serviceId);
                            break;
                        case 6:
                            Console.Write("Enter Vehicle ID to Complete Service: ");
                            int completeId = Convert.ToInt32(Console.ReadLine());
                            rentalService.CompleteService(completeId);
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the menu options.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");

                }
            }
        }
    }


}