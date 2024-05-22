using LoyalLib.Entities;

namespace LoyalLib.Repositories;

public interface IChallengeRepository
{
    Task<ChallengeEntity?> GetChallengeById(int id);

    Task<IEnumerable<ChallengeEntity>> GetAllChallenges();

    Task RemoveChallenge(ChallengeEntity book);

    Task UpdateChallenge(ChallengeEntity book);

    Task<ChallengeEntity> CreateChallenge(ChallengeEntity book);

    IQueryable<ChallengeEntity> GetQuery();

    Task<IEnumerable<ChallengeEntity>> GetUserChallenges(UserEntity userEntity);

    Task SaveChanges();
}