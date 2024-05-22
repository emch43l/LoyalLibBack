using LoyalLib.Db;
using LoyalLib.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Repositories;

public class ChallengeRepository : IChallengeRepository
{
    private readonly DatabaseContext _context;

    public ChallengeRepository(DatabaseContext context)
    {
        _context = context;
        context.ChangeTracker.LazyLoadingEnabled = false;
    }

    public async Task<ChallengeEntity?> GetChallengeById(int id)
    {
        return await _context.Challeges.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ChallengeEntity>> GetAllChallenges()
    {
        return await _context.Challeges.ToListAsync();
    }

    public async Task RemoveChallenge(ChallengeEntity challenge)
    {
        await Task.FromResult(_context.Challeges.Remove(challenge));
    }

    public async Task UpdateChallenge(ChallengeEntity challenge)
    {
        await Task.FromResult(_context.Challeges.Update(challenge));
    }

    public async Task<ChallengeEntity> CreateChallenge(ChallengeEntity challenge)
    {
        return (await _context.Challeges.AddAsync(challenge)).Entity;
    }

    public IQueryable<ChallengeEntity> GetQuery()
    {
        return _context.Challeges;
    }

    public async Task<IEnumerable<ChallengeEntity>> GetUserChallenges(UserEntity userEntity)
    {
        return await _context.Challeges.Where(c => c.User == userEntity).Include(c => c.RequiredBooks).ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}