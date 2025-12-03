using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PetAdoptionSystemApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Role Role { get; set; }
        public string Address { get; set; }
        public DateTime DateRegistered { get; set; }
        public User()
        {
            DateRegistered = DateTime.Now;
        }
        public ICollection<Adoption> Adoptions { get; set; }
     
    }
    public enum Role
    {
        Admin,
        Adopter
    }
   
}
