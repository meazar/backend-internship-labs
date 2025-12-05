using CustomerSupport.API.DTOs;
using CustomerSupport.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin,agent,customer")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(
        INotificationService service,
        ILogger<NotificationsController> logger
    )
    {
        _service = service;
        _logger = logger;
    }

    //GET: get notifications for a specific user
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int userId,
        [FromQuery] bool onlyUnread = false
    )
    {
        if (userId <= 0)
            return BadRequest("Valid userId must be provided.");

        var notifications = await _service.GetNotificationsByUserIdAsync(userId, onlyUnread);
        if (!notifications.Any())
            return NotFound("No notifications found for the specified user.");
        return Ok(notifications);
    }

    //POST: create a new notification
    [HttpPost]
    public async Task<IActionResult> CreateNotification(
        [FromQuery] int userId,
        [FromBody] NotificationCreateDto dto
    )
    {
        if (userId <= 0)
            return BadRequest("Valid userId must be provided.");
        if (dto == null)
            return BadRequest("Body is provided.");

        try
        {
            var created = await _service.CreateNotificationAsync(userId, dto);
            return CreatedAtAction(
                nameof(GetNotifications),
                new { userId = created.UserId },
                created
            );
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Create notification failed due to missing resources.");
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a notification.");
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    //PATCH: mark a notification as read
    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        if (id <= 0)
            return BadRequest("Valid notificationId must be provided.");

        var success = await _service.MarkAsReadAsync(id);
        if (!success)
            return NotFound("Notification not found.");

        return NoContent();
    }
}
