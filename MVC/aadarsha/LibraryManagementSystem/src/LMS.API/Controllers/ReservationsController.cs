using LMS.Core.DTOs.Requests;
using LMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(
        IReservationService reservationService,
        ILogger<ReservationsController> logger
    )
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.CreateReservationAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetReservationById), new { id = result.Data?.Id }, result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetReservationById(int id)
    {
        var result = await _reservationService.GetReservationByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("member/{memberId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetReservationsByMember(int memberId)
    {
        var result = await _reservationService.GetReservationsByMemberAsync(memberId);
        return Ok(result);
    }

    [HttpGet("book/{bookId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetReservationsByBook(int bookId)
    {
        var result = await _reservationService.GetReservationsByBookAsync(bookId);
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var result = await _reservationService.CancelReservationAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("{id}/fulfill")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> FulfillReservation(int id)
    {
        var result = await _reservationService.FulfillReservationAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("book/{bookId}/notify-next")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> NotifyNextInQueue(int bookId)
    {
        var result = await _reservationService.NotifyNextInQueueAsync(bookId);
        return Ok(result);
    }

    [HttpGet("{id}/queue-position")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetQueuePosition(int id)
    {
        var result = await _reservationService.GetQueuePositionAsync(id);
        return Ok(result);
    }

    [HttpGet("check-eligibility")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> CheckReservationEligibility(
        [FromQuery] int memberId,
        [FromQuery] int bookId
    )
    {
        var result = await _reservationService.CheckReservationEligibilityAsync(memberId, bookId);
        return Ok(result);
    }
}
