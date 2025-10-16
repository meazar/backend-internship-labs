// Models/User.cs

namespace TripManagementSystem.Models
{
    public class User
    {
        private static int _idSeq = 1000; // simple id generator

        public int UserId { get; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";

        // Constructor
        public User(string name, string email)
        {
            UserId = System.Threading.Interlocked.Increment(ref _idSeq);
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"{UserId}: {Name} - ({Email})";
        }
    }
}