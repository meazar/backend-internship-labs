using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { 
                message = "Library Management System API is running!", 
                timestamp = DateTime.UtcNow,
                version = "1.0"
            });
        }
    }
}