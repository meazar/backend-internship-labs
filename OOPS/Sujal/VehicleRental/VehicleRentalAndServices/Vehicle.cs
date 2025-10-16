using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalAndServices
{
    public abstract class Vehicle
    {
        public int id { get; set; }

        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool IsRented { get; set; }

        public bool IsUnderService { get; set; }
        public decimal RentalPricePerDay { get; set; }

        public Vehicle(int id, string type, string brand, string model, decimal rentalPricePerDay)
        {
            this.id = id;
            this.Type = type;
            this.Brand = brand;
            this.Model = model;
            this.IsRented = false;
            this.IsUnderService = false;
            this.RentalPricePerDay = rentalPricePerDay;
        }

        public abstract void DisplayInfo();


        public class Car : Vehicle
        {
            public Car(int id, string brand, string model, decimal rentalPricePerDay)
                : base(id, "Car", brand, model, rentalPricePerDay)
            {
            }
            public override void DisplayInfo()
            {
                Console.WriteLine($"[Car]ID: {id}, Type: {Type}, Brand: {Brand}, Model: {Model}, Price/Day: {RentalPricePerDay:C}, Rented: {IsRented}, Under Service: {IsUnderService}");
            }
        }

        public class Bike : Vehicle
        {
            public Bike(int id, string brand, string model, decimal rentalPricePerDay)
                : base(id, "Bike", brand, model, rentalPricePerDay)
            {
            }
            public override void DisplayInfo()
            {
                Console.WriteLine($"[Bike]ID: {id}, Type: {Type}, Brand: {Brand}, Model: {Model}, Price/Day: {RentalPricePerDay:C}, Rented: {IsRented}, Under Service: {IsUnderService}");
            }


        }

        public class  Scooter : Vehicle
        {
            
            public Scooter(int id, string brand, string model, decimal rentalPricePerDay)
                : base(id, "Scooter", brand, model, rentalPricePerDay)
            {
            }
            public override void DisplayInfo()
            {
                Console.WriteLine($"[Scooter]ID: {id}, Type: {Type}, Brand: {Brand}, Model: {Model}, Price/Day: {RentalPricePerDay:C}, Rented: {IsRented}, Under Service: {IsUnderService}");
            }   
        }
    }

}

