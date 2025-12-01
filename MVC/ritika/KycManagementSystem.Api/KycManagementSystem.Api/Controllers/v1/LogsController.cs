using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/logs")]
    [Authorize(Roles = "Admin")]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository _logsRepo;

        public LogsController(ILogsRepository logsRepo)
        {
            _logsRepo = logsRepo;
        }

        [HttpPost("paged")]
        public async Task<IActionResult> GetPagedLogs([FromBody] PageRequest request)
        {
            var result = await _logsRepo.GetPagedLogsAsync(request);
            return Ok(ApiResponse<PageResult<ApiLog>>.Ok(result));
        }

    }
}
