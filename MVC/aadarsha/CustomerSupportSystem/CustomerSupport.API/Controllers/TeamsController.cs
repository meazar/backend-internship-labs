using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using CustomerSupport.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _service;
    private readonly ILogger<TeamsController> _logger;

    public TeamsController(ITeamService service, ILogger<TeamsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: get all teams
    [HttpGet]
    [Authorize(Roles = "admin,agent")]
    public async Task<IActionResult> GetAllTeams()
    {
        var teams = await _service.GetAllTeamsAsync();
        return Ok(teams);
    }

    // GET: get team by id
    [HttpGet("{id}")]
    [Authorize(Roles = "admin,agent")]
    public async Task<IActionResult> GetTeamById(int id)
    {
        var team = await _service.GetTeamByIdAsync(id);
        if (team == null)
            return NotFound($"Team with id {id} not found.");
        return Ok(team);
    }

    // POST: create a new team
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateTeam([FromBody] TeamCreateDto dto)
    {
        try
        {
            var team = new Team { Name = dto.Name };
            var createdTeam = await _service.CreateTeamAsync(team);
            return CreatedAtAction(nameof(GetTeamById), new { id = createdTeam.Id }, createdTeam);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating team");
            return StatusCode(500, "Internal server error");
        }
    }

    // PUT: update an existing team
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamUpdateDto dto)
    {
        try
        {
            var existingTeam = await _service.GetTeamByIdAsync(id);
            if (existingTeam == null)
                return NotFound($"Team with id {id} not found.");
            existingTeam.Name = dto.Name;
            var updatedTeam = await _service.UpdateTeamAsync(existingTeam);
            if (updatedTeam == null)
                return StatusCode(500, "Failed to update team.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating team with id {TeamId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: delete a team
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        try
        {
            var existingTeam = await _service.GetTeamByIdAsync(id);
            if (existingTeam == null)
                return NotFound($"Team with id {id} not found.");
            var deleted = await _service.DeleteTeamAsync(id);
            if (!deleted)
                return StatusCode(500, "Failed to delete team.");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting team with id {TeamId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
