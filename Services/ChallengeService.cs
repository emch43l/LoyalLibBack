using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LoyalLib.DTO.Request;
using LoyalLib.DTO.Response;
using LoyalLib.Entities;
using LoyalLib.Exception;
using LoyalLib.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Services;

public class ChallengeService : IChallengeService
{
    private readonly UserManager<UserEntity> _userManager;

    private readonly IChallengeRepository _challengeRepository;

    private readonly IBookRepository _bookRepository;
    
    public ChallengeService(IChallengeRepository challengeRepository, UserManager<UserEntity> userManager, IBookRepository bookRepository)
    {
        _challengeRepository = challengeRepository;
        _userManager = userManager;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<ChallengeResponse>> GetUserChallenges(ClaimsPrincipal principal)
    {
        UserEntity? userEntity = await _userManager.GetUserAsync(principal);
        if (userEntity == null)
            throw new UserNotFoundException();
        
        return await _challengeRepository.GetQuery().Where(c => c.User == userEntity).Select(challenge => new ChallengeResponse()
        {
            Id = challenge.Id,
            Name = challenge.Name,
            Points = challenge.Points,
            Books = challenge.RequiredBooks.Select(book => new ChallengeBookResponse()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                IsCompleted = book.UsersWhoRead.Contains(userEntity),
                Url = book.Url,
                
            }).ToList()
        }).ToListAsync();
    }

    public async Task MarkBookRead(ReadBookRequest request, ClaimsPrincipal principal)
    {
        UserEntity? userEntity = await _userManager.GetUserAsync(principal);
        if (userEntity == null)
            throw new UserNotFoundException();

        BookEntity? bookEntity = await _bookRepository.GetBookById(request.BookId);
        if (bookEntity == null)
            throw new BookNotFoundException();

        if (bookEntity.UsersWhoRead.FirstOrDefault(u => u.Id == userEntity.Id) == null)
        {
            bookEntity.UsersWhoRead.Add(userEntity);
            await _bookRepository.SaveChanges();
        } 
        
    }

    public async Task GenerateChallenge(ClaimsPrincipal principal)
    {
        UserEntity? userEntity = await _userManager.GetUserAsync(principal);
        if (userEntity == null)
            throw new UserNotFoundException();

        Random random = new Random();

        ChallengeEntity challengeEntity = new ChallengeEntity()
        {
            Name = $"Generated challenge {random.Next(0,2137)}",
            Points = random.Next(20, 200),
            User = userEntity,
            RequiredBooks = await _bookRepository.GetQuery().OrderBy(b =>EF.Functions.Random()).Take(random.Next(1, 5))
                .ToListAsync()
        };

        await _challengeRepository.CreateChallenge(challengeEntity);
        await _challengeRepository.SaveChanges();
    }
}