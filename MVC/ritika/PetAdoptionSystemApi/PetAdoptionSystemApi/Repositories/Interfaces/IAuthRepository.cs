namespace PetAdoptionSystemApi.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<string> Register(string fullName, string password, string role, string email, string address, string phone);
        public Task<string> Login(string email, string password);
    }
}
