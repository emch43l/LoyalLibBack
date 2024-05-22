using Microsoft.AspNetCore.Identity;

namespace LoyalLib.Entities;

public class UserEntity : IdentityUser<int>
{
    public RefreshTokenEntity? RefreshToken { get; set; }

    public ICollection<ChallengeEntity> Challenges { get; set; }

    public ICollection<BookEntity> BooksRead { get; set; }
}