using PetAdoptionSystemApi.Data;
using PetAdoptionSystemApi.Repositories.Interfaces;

namespace PetAdoptionSystemApi.Repositories.Implementations
{
    public class AuthRepository: IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public Task<string> Login(string email, string password)
        {
            throw new NotImplementedException();
        }
        public Task<string> Register(string fullName, string password, string role, string email, string address, string phone)
        {
            throw new NotImplementedException();
        }
    }
}
