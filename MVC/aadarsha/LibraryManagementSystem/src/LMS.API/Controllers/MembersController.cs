using LMS.Core.DTOs.Requests;
using LMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly ILogger<MembersController> _logger;

    public MembersController(IMemberService memberService, ILogger<MembersController> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    [HttpPost("create/{userId}")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> CreateMember(int userId)
    {
        var result = await _memberService.CreateMemberAsync(userId);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetMemberById), new { id = result.Data?.Id }, result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetMemberById(int id)
    {
        var result = await _memberService.GetMemberByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetMemberByUserId(int userId)
    {
        var result = await _memberService.GetMemberByUserIdAsync(userId);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("member-id/{memberId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetMemberByMemberId(string memberId)
    {
        var result = await _memberService.GetMemberByMemberIdAsync(memberId);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> GetAllMembers()
    {
        var result = await _memberService.GetAllMembersAsync();
        return Ok(result);
    }

    [HttpGet("active")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> GetActiveMembers()
    {
        var result = await _memberService.GetActiveMembersAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _memberService.UpdateMemberAsync(id, request);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id}/deactivate")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> DeactivateMember(int id)
    {
        var result = await _memberService.DeactivateMemberAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id}/activate")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> ActivateMember(int id)
    {
        var result = await _memberService.ActivateMemberAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}/status")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetMemberStatus(int id)
    {
        var result = await _memberService.GetMemberStatusAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}/can-borrow")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> CanBorrowMoreBooks(int id)
    {
        var result = await _memberService.CanBorrowMoreBooksAsync(id);
        return Ok(result);
    }
}
