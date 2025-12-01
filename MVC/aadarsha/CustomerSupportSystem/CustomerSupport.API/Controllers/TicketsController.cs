using System.Security.Claims;
using CustomerSupport.API.DTOs;
using CustomerSupport.API.Models;
using CustomerSupport.API.Repositories;
using CustomerSupport.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketRepository _tickets;
    private readonly IMessageRepository _messages;
    private readonly IAutoAssignmentService _autoAssignment;
    private readonly ILogger<TicketsController> _logger;

    public TicketsController(
        ITicketRepository tickets,
        IMessageRepository messages,
        IAutoAssignmentService autoAssignment,
        ILogger<TicketsController> logger
    )
    {
        _tickets = tickets;
        _messages = messages;
        _autoAssignment = autoAssignment;
        _logger = logger;
    }

    // GET: all tickets
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? customerId)
    {
        var tickets = await _tickets.GetAllTicketsAsync(customerId);

        if (!tickets.Any())
            return NotFound();

        return Ok(tickets);
    }

    // GET: ticket by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = await _tickets.GetByIdAsync(id);
        if (ticket == null)
            return NotFound();

        var messages = await _messages.GetMessageByTicket(id);

        return Ok(new { ticket, messages });
    }

    // POST: create ticket
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketCreateDto dto)
    {
        var customerId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

        if (customerId == 0)
            return Unauthorized("User not found.");

        var ticket = new Ticket
        {
            Subject = dto.Subject,
            Description = dto.Description,
            CustomerId = customerId,
            TeamId = null,
            Status = "open",
            AssignedAgentId = null, // will be auto-assigned
            CreatedAt = DateTime.UtcNow,
        };

        var ticketId = await _tickets.CreateAsync(ticket);

        var assigned = await _autoAssignment.AutoAssignTicketAsync(ticketId);

        var response = new
        {
            ticketId,
            agentAssigned = assigned,
            message = assigned
                ? "Ticket created and agent assigned successfully."
                : "Ticket created - awaiting agent assignment (no available agents)",
        };
        return CreatedAtAction(nameof(GetTicket), new { id = ticketId }, response);
    }

    // POST: assign agent to ticket
    [Authorize(Roles = "admin,agent")]
    [HttpPost("{id}/assign")]
    public async Task<IActionResult> AssignAgent(int id, [FromQuery] int agentId)
    {
        await _tickets.AssignAgentAsync(id, agentId);

        return NoContent();
    }

    // POST: assign team to ticket
    [Authorize(Roles = "admin,agent")]
    [HttpPost("{ticketId}/assign-team")]
    public async Task<IActionResult> AssignTeamToTicket(int ticketId, [FromQuery] int teamId)
    {
        try
        {
            var teamExists = await _tickets.TeamExistsAsync(teamId);
            if (!teamExists)
                return BadRequest(new { error = "Team does not exist." });

            var ticket = await _tickets.GetByIdAsync(ticketId);
            if (ticket == null)
                return NotFound(new { error = "Ticket not found." });

            if (ticket.Status == "resolved" || ticket.Status == "closed")
                return BadRequest(
                    new { error = $"Cannot reassign team for {ticket.Status} tickets." }
                );

            if (ticket.TeamId == teamId)
                return Conflict(new { error = "Ticket is already assigned to this team." });

            var updated = await _tickets.AssignTeamAsync(ticketId, teamId);
            if (!updated)
                return StatusCode(500, new { error = "Failed to assign team to ticket." });

            return Ok(
                new
                {
                    message = "Team assigned successfully.",
                    previousTeamId = ticket.TeamId,
                    newTeamId = teamId,
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error assigning team {TeamId} to ticket with id {TicketId}",
                teamId,
                ticketId
            );
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: add message to ticket
    [HttpPost("{id}/messages")]
    public async Task<IActionResult> AddMessage(int id, [FromBody] TicketMessage message)
    {
        if (message == null || string.IsNullOrWhiteSpace(message.Content))
            return BadRequest("Message cannot be empty.");

        message.TicketId = id;
        var result = await _messages.AddMessageAsync(id, message.Content, message.AuthorId);

        if (result == 0)
            return BadRequest("Unable to save message.");

        return Ok(new { message = "Message added successfully." });
    }

    // PUT: update ticket status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
    {
        var updated = await _tickets.UpdateTicketStatusAsync(id, status);
        if (!updated)
            return NotFound();

        return Ok(new { message = "Ticket status updated successfully." });
    }
}
