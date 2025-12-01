using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.API.Controllers;

[ApiController]
[Route("api/tickets/{ticketId}/[controller]")]
[Authorize(Roles = "admin,agent,customer")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _service;

    public MessagesController(IMessageService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get(int ticketId)
    {
        var message = await _service.GetMessagesForTicketAsync(ticketId);
        if (!message.Any())
            return NotFound();
        return Ok(message);
    }
}
