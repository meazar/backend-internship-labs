using LMS.Core.DTOs.Requests;
using LMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class FinesController : ControllerBase
{
    private readonly IFineService _fineService;
    private readonly ILogger<FinesController> _logger;

    public FinesController(IFineService fineService, ILogger<FinesController> logger)
    {
        _fineService = fineService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> CreateFine([FromBody] CreateFineRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _fineService.CreateFineAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetFineById(int id)
    {
        var result = await _fineService.GetFineByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("member/{memberId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetFinesByMember(int memberId)
    {
        var result = await _fineService.GetFinesByMemberAsync(memberId);
        return Ok(result);
    }

    [HttpGet("pending")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetPendingFines()
    {
        var result = await _fineService.GetPendingFinesAsync();
        return Ok(result);
    }

    [HttpPost("{id}/pay")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> PayFine(int id, [FromBody] PayFineRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _fineService.PayFineAsync(id, request.Amount);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("{id}/waive")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> WaiveFine(int id, [FromBody] string reason)
    {
        var result = await _fineService.WaiveFineAsync(id, reason);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("member/{memberId}/total-pending")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetTotalPendingFines(int memberId)
    {
        var result = await _fineService.GetTotalPendingFinesAsync(memberId);
        return Ok(result);
    }

    [HttpPost("generate-overdue-fines")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> GenerateOverdueFines()
    {
        var result = await _fineService.GenerateOverdueFinesAsync();
        return Ok(result);
    }

    [HttpGet("member/{memberId}/summary")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetFineSummary(int memberId)
    {
        var result = await _fineService.GetFineSummaryAsync(memberId);
        return Ok(result);
    }
}
