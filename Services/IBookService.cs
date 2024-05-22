using System.Security.Claims;
using LoyalLib.DTO.Response;
using LoyalLib.Entities;

namespace LoyalLib.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponse>> GetBooks(ClaimsPrincipal principal);

    Task<BookResponse> GetBookById(ClaimsPrincipal principal, int id);

    Task<IEnumerable<BookResponse>> GetBooksPaginated(ClaimsPrincipal principal, string? query, int page);
}