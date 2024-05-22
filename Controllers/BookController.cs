using LoyalLib.DTO.Response;
using LoyalLib.Entities;
using LoyalLib.Repositories;
using LoyalLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoyalLib.Controllers;

[Authorize]
[Route("books")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [Route("single/{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        BookResponse result = await _bookService.GetBookById(HttpContext.User,id);
        return Ok(result);
    }

    [HttpGet]
    [Route("{pageNumber}")]
    public async Task<IActionResult> GetBooks([FromQuery] string? query, int pageNumber)
    {
        IEnumerable<BookResponse> result = await _bookService.GetBooksPaginated(HttpContext.User,query,pageNumber);
        return Ok(result);
    }
    
}