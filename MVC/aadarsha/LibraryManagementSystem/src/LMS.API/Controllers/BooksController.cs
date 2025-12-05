using LMS.Core.DTOs.Requests;
using LMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpGet("get-allbooks")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetAllBooks(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await _bookService.GetAllBooksAsync(pageNumber, pageSize);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("getbook/id/{id}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var result = await _bookService.GetBookByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("getbook/isbn/{isbn}")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetBookByIsbn(string isbn)
    {
        var result = await _bookService.GetBookByIsbnAsync(isbn);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("search")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> SearchBooks(
        [FromQuery] string texts,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        if (string.IsNullOrWhiteSpace(texts))
            return BadRequest("Search word is required.");

        var result = await _bookService.SearchBooksAsync(texts);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("getbook/id/{id}/available-copies")]
    [Authorize(Policy = "MemberOrHigher")]
    public async Task<IActionResult> GetAvailableCopies(int id)
    {
        var result = await _bookService.GetAvailableCopiesAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("createbook")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookService.CreateBookAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetBookById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("updatebook/{id}")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] CreateBookRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookService.UpdateBookAsync(id, request);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("deletebook/{id}")]
    [Authorize(Policy = "LibrarianOrAdmin")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
