using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using CustomerSupport.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgentsController : ControllerBase
{
    private readonly IAgentService _service;
    private readonly ILogger<AgentsController> _logger;

    public AgentsController(IAgentService service, ILogger<AgentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: get all agents
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllAgents()
    {
        var agents = await _service.GetAllAgentsAsync();
        return Ok(agents);
    }

    // GET: get agent by id
    [HttpGet("{id}")]
    [Authorize(Roles = "admin,agent")]
    public async Task<IActionResult> GetAgentById(int id)
    {
        var agent = await _service.GetAgentByIdAsync(id);
        if (agent == null)
            return NotFound($"Agent with id {id} not found.");
        return Ok(agent);
    }

    // POST: create a new agent
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateAgent([FromBody] AgentCreateDto dto)
    {
        try
        {
            if (dto.TeamId == null)
                return BadRequest("TeamId is required.");

            var agent = new Agent
            {
                UserId = dto.UserId,
                TeamId = dto.TeamId.Value,
                IsAvailable = dto.IsAvailable,
                MaxTickets = dto.MaxTickets,
                CurrentTickets = 0,
            };

            var created = await _service.CreateAgentAsync(agent);
            return CreatedAtAction(nameof(GetAgentById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Create ageint failed validation");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create agent failed due to server error");
            return StatusCode(500, "An error occurred while creating the agent.");
        }
    }

    // PUT: update an existing agent
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateAgent(int id, [FromBody] AgentUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID in URL and body do not match.");

        var existingAgent = await _service.GetAgentByIdAsync(id);
        if (existingAgent == null)
            return NotFound($"Agent with id {id} not found.");

        existingAgent.TeamId = dto.TeamId ?? existingAgent.TeamId;
        existingAgent.IsAvailable = dto.IsAvailable;
        existingAgent.MaxTickets = dto.MaxTickets;

        var updated = await _service.UpdateAgentAsync(existingAgent);
        return updated != null
            ? NoContent()
            : StatusCode(500, "An error occurred while updating the agent.");
    }

    // DELETE: delete an agent
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteAgent(int id)
    {
        var existingAgent = await _service.GetAgentByIdAsync(id);
        if (existingAgent == null)
            return NotFound($"Agent with id {id} not found.");

        await _service.DeleteAgentAsync(id);
        return Ok("Agent deleted successfully.");
    }
}
