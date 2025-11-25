using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionManagementSystem.Models
{
    public enum UserRole
    {
        Admin,
        Adopter
    }
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateRegistered { get; set; }

        public User()
        {
            DateRegistered = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{FullName} ({Role}) - {Email}";
        }

        
    }
}
