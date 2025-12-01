using KycManagementSystem.Api.Models.DTOs.Notification;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin,Officer")]
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] NotificationDto dto)
        {
            await _service.SendNotificationAsync(dto);
            return Ok(ApiResponse<string>.Ok("Notification sent"));
        }
        [Authorize(Roles = "Admin,Officer,Client")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> List(int userId)
        {
            var result = await _service.GetNotificationsAsync(userId);
            return Ok(ApiResponse<object>.Ok(result));
        }
        [Authorize(Roles = "Admin,Officer,Client")]
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok(ApiResponse<string>.Ok("Marked as read"));
        }
        [Authorize(Roles = "Admin,Officer,Client")]
        [HttpPut("read-all/{userId}")]
        public async Task<IActionResult> MarkAll(int userId)
        {
            await _service.MarkAllAsReadAsync(userId);
            return Ok(ApiResponse<string>.Ok("All notifications marked as read"));
        }
    }
}
