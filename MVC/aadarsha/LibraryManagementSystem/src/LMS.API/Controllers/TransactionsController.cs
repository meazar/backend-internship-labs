using LMS.Core.DTOs.Requests;
using LMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(
        ITransactionService transactionService,
        ILogger<TransactionsController> logger
    )
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    [HttpPost("checkout")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> CheckoutBook([FromBody] CheckoutRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _transactionService.CheckoutBookAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("{id}/return")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var result = await _transactionService.ReturnBookAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("{id}/renew")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> RenewBook(int id)
    {
        var result = await _transactionService.RenewBookAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var result = await _transactionService.GetTransactionByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("member/{memberId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetTransactionsByMember(int memberId)
    {
        var result = await _transactionService.GetTransactionsByMemberAsync(memberId);
        return Ok(result);
    }

    [HttpGet("book/{bookId}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetTransactionsByBook(int bookId)
    {
        var result = await _transactionService.GetTransactionsByBookAsync(bookId);
        return Ok(result);
    }

    [HttpGet("overdue")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetOverdueTransactions()
    {
        var result = await _transactionService.GetOverdueTransactionsAsync();
        return Ok(result);
    }

    [HttpGet("member/{memberId}/active")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetActiveCheckouts(int memberId)
    {
        var result = await _transactionService.GetActiveCheckoutsAsync(memberId);
        return Ok(result);
    }

    [HttpGet("check-eligibility")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> CheckCheckoutEligibility(
        [FromQuery] int memberId,
        [FromQuery] int bookId
    )
    {
        var result = await _transactionService.CheckCheckoutEligibilityAsync(memberId, bookId);
        return Ok(result);
    }

    [HttpGet("stats")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> GetTransactionStats(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var result = await _transactionService.GetTransactionStatsAsync(start, end);
        return Ok(result);
    }
}
