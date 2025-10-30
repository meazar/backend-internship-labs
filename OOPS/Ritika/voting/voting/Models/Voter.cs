using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterRegistrationSystem.Models
{
    public class Voter : Person
    {
        public string VoterId { get; set; }

        public override void DisplayInfo()
        {
            Console.WriteLine($"VoterId : {VoterId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"CitizenshipId: {CitizenshipId}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine("-----------------------------------------------------");
        }
    }
}
