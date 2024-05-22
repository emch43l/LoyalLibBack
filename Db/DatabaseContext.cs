using LoyalLib.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Db;

public class DatabaseContext : IdentityDbContext<UserEntity,RoleEntity,int>
{
    public DbSet<BookEntity> Books { get; set; }
    
    public DbSet<RefreshTokenEntity> Tokens { get; set; }
    
    public DbSet<ChallengeEntity> Challeges { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
    
}