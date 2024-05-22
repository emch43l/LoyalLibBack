using System.Security.Claims;
using LoyalLib.DTO.Response;
using LoyalLib.Entities;
using LoyalLib.Exception;
using LoyalLib.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Services;

public class BookService : IBookService
{
    private const int MaxPerPage = 25;
    
    private readonly IBookRepository _bookRepository;

    private readonly UserManager<UserEntity> _userManager;

    public BookService(IBookRepository bookRepository, UserManager<UserEntity> userManager)
    {
        _bookRepository = bookRepository;
        _userManager = userManager;
    }

    public async Task<IEnumerable<BookResponse>> GetBooks(ClaimsPrincipal principal)
    {
        UserEntity userEntity = await GetUserOrThrow(principal);
        
        return _bookRepository.GetQuery().Select(b => new BookResponse()
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Cover = b.Cover,
            CoverColor = b.CoverColor,
            CoverThumb = b.CoverThumb,
            Epoch = b.Epoch,
            Genre = b.Genre,
            Href = b.Href,
            Kind = b.Kind,
            SimpleThumb = b.SimpleThumb,
            ReadByUser = b.UsersWhoRead.Contains(userEntity)
        });
    }

    public async Task<BookResponse> GetBookById(ClaimsPrincipal principal, int id)
    {
        UserEntity userEntity = await GetUserOrThrow(principal);
        
        return 
            await _bookRepository.GetQuery().Select(b => new BookResponse()
            {
                Id = b.Id,
                Kind = b.Kind,
                Title = b.Title,
                Url = b.Url,
                CoverColor = b.CoverColor,
                Author = b.Author,
                Cover = b.Cover,
                Epoch = b.Epoch,
                Href = b.Href,
                Genre = b.Genre,
                SimpleThumb = b.SimpleThumb,
                Slug = b.Slug,
                CoverThumb = b.CoverThumb,
                ReadByUser = b.UsersWhoRead.Contains(userEntity)
            }).FirstOrDefaultAsync(b => b.Id == id) ?? 
            throw new BookNotFoundException();
    }

    public async Task<IEnumerable<BookResponse>> GetBooksPaginated(ClaimsPrincipal principal, string? query = null, int page = 1)
    {

        UserEntity userEntity = await GetUserOrThrow(principal);
        
        if (page < 1)
            throw new ArgumentException();
        
        return await _bookRepository
            .GetQuery()
            .Where(b => query == null || b.Title.Contains(query))
            .OrderBy(b => b.Id)
            .Skip((page - 1) * MaxPerPage)
            .Take(MaxPerPage)
            .Select(b => new BookResponse()
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Cover = b.Cover,
                CoverColor = b.CoverColor,
                CoverThumb = b.CoverThumb,
                Epoch = b.Epoch,
                Genre = b.Genre,
                Href = b.Href,
                Kind = b.Kind,
                SimpleThumb = b.SimpleThumb,
                ReadByUser = b.UsersWhoRead.Contains(userEntity)
            }).ToListAsync();
    }

    private async Task<UserEntity> GetUserOrThrow(ClaimsPrincipal principal)
    {
        UserEntity? userEntity = await _userManager.GetUserAsync(principal);
        if (userEntity == null)
            throw new UserNotFoundException();
        return userEntity;
    }
}