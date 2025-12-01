using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationController> _logger;


        public NotificationController(INotificationRepository notificationRepository, ILogger <NotificationController>logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;


        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            try
            {
                var notification = await _notificationRepository.GetUserNotificationsAsync(userId);
                return Ok(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR IN GETTTING USER NOtification");
                return StatusCode(500, new {message = "Internal server error"});
            }
        }

        [HttpGet("user/{userId}/unread-count")]

        public async Task<IActionResult> GetUnreadNotificationCount(int userId)
        {
            try
            {
                var count = await _notificationRepository.GetUnreadNotificationCountAsync(userId);
                return Ok(new { unreadCunt = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error getting unread notification count");
                return StatusCode(500, new { message = "Internal server error" });
            }


        }

        [HttpPost("{notificationId}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            try
            {
                var success = await _notificationRepository.MarkNotificationAsReadAsync(notificationId);

                if (!success)
                {
                    return NotFound(new { message = "Notification is not found" });
                }
                return Ok(new {message = "Notification marked as read"});

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error making notification as read");
                return StatusCode(500, new { message = "Internal Server error" });  
            }
        }

        [HttpPost("user/{userId}/mark-all-read")]
        public async Task<IActionResult> MarkAllNotificationAsRead(int userId)
        {
            try
            {
                var success = await _notificationRepository.MarkAllNotificationsAsReadAsync(userId);
                return Ok(new { message = "All notification marked as read" });

            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error in making all notification as read");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("send")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Message = request.Message,
                    Type = request.Type,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var notificationId = await _notificationRepository.CreateNotificationAsync(notification);

                if (notificationId > 0)
                {
                    return Ok(new { message = "Send Notification To ",notificationId });
                }
                return StatusCode(500, new { message = "Fail to send Notification" });

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in sending notification");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
              
    }
}
