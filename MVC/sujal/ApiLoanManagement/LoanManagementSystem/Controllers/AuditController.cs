/*using LoanManagementSystem.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : Controller
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<AuditController> _logger;


        public AuditController(IAuditRepository auditRepository, ILogger<AuditController> logger)
        {
            _auditRepository = auditRepository;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                var auditlogs = await _auditRepository.GetAuditLogsAsync(fromDate, toDate);
                return Ok(auditlogs);

            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audit logs");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpGet("user/{userId}")]

        public async Task<IActionResult> GetAuditLogsByUserAsync(int userId)
        {
            try
            {
                var audilogs = await _auditRepository.GetAuditLogsByUserAsync(userId);
                return Ok(audilogs);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting user audit logs");
                return StatusCode(500, new { messsage = "Internal Server ERROR" });
            }

        }

        [HttpGet("table/{tableName}")]

        public async Task<IActionResult> GetAuditLogsByTable(string tableName)
        {
            try
            {
                var auditLog = await _auditRepository.GetAuditLogsByTableAsync(tableName);
                return Ok(auditLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting table audit logs");
                return StatusCode(500, new { message = "INternal Server ERROR" });
            }

        }


    }
}
*/