using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionManagementSystem.Models
{
    public class Adoption
    {
        public int AdoptionId { get; set; }
        public int PetId { get; set; }
        public int UserId { get; set; }
        public DateTime AdoptionDate { get; set; }

        public Adoption()
        {
            AdoptionDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Adoption #{AdoptionId} -> PetId: {PetId}, UserId: {UserId}, Date: {AdoptionDate}";
        }
    }
}
