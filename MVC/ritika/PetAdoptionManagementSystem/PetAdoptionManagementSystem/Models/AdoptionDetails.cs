using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionManagementSystem.Models
{
    public class AdoptionDetails
    {
        public int AdoptionId { get; set; }
        public string PetName { get; set; }
        public string AdopterName { get; set; }
        public DateTime AdoptionDate { get; set; }

        public override string ToString()
        {
            return $"Adoption #{AdoptionId} - {PetName} adopted By {AdopterName} on {AdoptionDate}";
        }
    }
}
