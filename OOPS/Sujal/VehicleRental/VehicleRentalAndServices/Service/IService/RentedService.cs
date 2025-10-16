using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalAndServices.Service.IService
{
    public class RentedService : IVehicle
    {
        private List<Vehicle> vehicles;
        public RentedService(List<Vehicle> vehicles)
        {
            this.vehicles = vehicles;
        }
        public void RentVehicle(int id)
        {
            try
            {
                var vehicle = vehicles.FirstOrDefault(v => v.id == id && !v.IsRented && !v.IsUnderService);
                if (vehicle != null)
                {
                    vehicle.IsRented = true;
                    Console.WriteLine($"Vehicle  {vehicle.id}||{vehicle.Type}||{vehicle.Model}||{vehicle.Brand}has been rented.");
                }
                else
                {
                    throw new Exception("Vehicle not available for rent.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");


            }
        }

        public void ReturnVehicle(int id)
        {
            try
            {
                var vehicle = vehicles.FirstOrDefault(v => v.id == id);
                if (vehicle != null && vehicle.IsRented)
                {
                    vehicle.IsRented = false;
                    Console.WriteLine($"Vehicle {vehicle.id}||{vehicle.Type}||{vehicle.Model}||{vehicle.Brand} has been returned.");
                }
                else
                {
                    throw new Exception("Vehicle not currently rented.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void SendForService(int id)
        {
            try
            {
                var vehicle = vehicles.FirstOrDefault(v => v.id == id);
                if (vehicle != null && !vehicle.IsUnderService && !vehicle.IsRented)
                {
                    vehicle.IsUnderService = true;
                    Console.WriteLine($"Vehicle {vehicle.id}||{vehicle.Type}||{vehicle.Model}||{vehicle.Brand} has been sent for service.");
                }
                else
                {
                    throw new Exception("Vehicle not available for service.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void CompleteService(int id)
        {
            try
            {
                var vehicle = vehicles.FirstOrDefault(v => v.id == id);
                if (vehicle != null && vehicle.IsUnderService)
                {
                    vehicle.IsUnderService = false;
                    Console.WriteLine($"Vehicle {vehicle.id}||{vehicle.Type}||{vehicle.Model}||{vehicle.Brand} has completed service.");
                }
                else
                {
                    throw new Exception("Vehicle not currently under service.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void DisplayAvailableVehicles()
        {
            var availableVehicles = vehicles.Where(v => !v.IsRented && !v.IsUnderService).ToList();
            if (availableVehicles.Any())
            {
                Console.WriteLine("Available Vehicles:");
                foreach (var vehicle in availableVehicles)
                {
                    vehicle.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine("No vehicles available.");
            }
        }

        public void DisplayserviceVehicles()
        {
            var serviceVehicles = vehicles.Where(v => v.IsUnderService).ToList();
            if (serviceVehicles.Any())
            {
                Console.WriteLine("Vehicles Under Service:");
                foreach (var vehicle in serviceVehicles)
                {
                    vehicle.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine("No vehicles currently under service.");
            }
        }



    }
}
