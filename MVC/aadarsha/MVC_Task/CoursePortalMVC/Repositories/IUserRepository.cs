using CoursePortalMVC.Models;

namespace CoursePortalMVC.Repositories
{
    public interface IUserRepository
    {
        User? ValidateUser(string email, string password);
    }
}