using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.DTOs.Users;

namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserDto dto);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<PageResult<UserDto>> GetPagedUsersAsync(PageRequest request);


        Task UpdateUserAsync(UserDto dto);
        Task DeleteUserAsync(int id);
    }
}
