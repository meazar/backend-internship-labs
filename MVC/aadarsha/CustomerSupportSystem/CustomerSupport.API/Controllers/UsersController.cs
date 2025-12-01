using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    public UsersController(IUserRepository userRepo) => _userRepo = userRepo;

    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userRepo.GetUserById(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepo.GetAllUsers();
        if (!users.Any())
            return NotFound();
        return Ok(users);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        // NOTE: In a real system hash passwords, validate input, return DTOs not entities
        var id = await _userRepo.CreateUser(user);
        return CreatedAtAction(nameof(GetUser), new { id }, user);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepo.GetUserById(id);
        if (user == null)
            return NotFound();
        await _userRepo.DeleteUser(id);
        return Ok("User deleted successfully.");
    }
}
