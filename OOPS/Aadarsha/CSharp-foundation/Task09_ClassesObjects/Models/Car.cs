namespace Task09_ClassesObjects.Models
{
    using System;
    public class Car
    {
        // fields
        public string? brand; // public is an access modifier which allows access to the field from outside the class
        public string? model; // string? indicates that the string can be null and is a nullable type
        public int? year; 

        // method(behavior)
        public void DisplayInfo()
        {
            Console.WriteLine($"Brand: {brand}, Model: {model}, Year: {year}");
        }
    }
}