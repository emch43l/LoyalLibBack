using LoyalLib.DTO.Response;
using LoyalLib.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Services;

public class RankingService : IRankingService
{
    private readonly UserManager<UserEntity> _userManager;

    public RankingService(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<RankingResponse>> GetRanking()
    {
        // this query generates like 50 lines of SQL code in console :)))))))
        return await _userManager.Users.Select(u => new RankingResponse()
        {
            UserName = u.UserName!,
            PointsCount = u.Challenges.Where(c => c.RequiredBooks.Count(b => b.UsersWhoRead.Contains(u)) == c.RequiredBooks.Count()).Sum(c => c.Points),
            CompletedChallengesCount = u.Challenges.Count(c => c.RequiredBooks.Count(b => b.UsersWhoRead.Contains(u)) == c.RequiredBooks.Count()),
            ReadBooksCount = u.BooksRead.Count()
        }).ToListAsync();
    }
}