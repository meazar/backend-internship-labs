using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.DTOs.Users;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Connections;


namespace KycManagementSystem.Api.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserService(IUserRepository userRepo, ISqlConnectionFactory connectionFactory)
        {
            _userRepo = userRepo;
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = PasswordHasher.Hash(dto.Password),
                Role = dto.Role,
                IsActive = true
            };
            return await _userRepo.CreateUserAsync(user);
        }


        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive
            });
        }

        public async Task<PageResult<UserDto>> GetPagedUsersAsync(PageRequest request)
        {
            using var conn = _connectionFactory.CreateConnection();

            string sql = @"
        SELECT * FROM Users
        ORDER BY Id
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(*) FROM Users;
    ";

            using var multi = await conn.QueryMultipleAsync(sql, new
            {
                Offset = (request.PageNumber - 1) * request.PageSize,
                PageSize = request.PageSize
            });

            var users = (await multi.ReadAsync<User>()).ToList();
            var totalCount = await multi.ReadSingleAsync<int>();

            var dtoList = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive
            }).ToList();

            return new PageResult<UserDto>
            {
                Items = dtoList,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }



        public async Task UpdateUserAsync(UserDto dto)
        {
            var user = new User
            {
                Id = dto.Id,
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                IsActive = dto.IsActive
            };
            await _userRepo.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepo.DeleteUserAsync(id);
        }
    }
}
