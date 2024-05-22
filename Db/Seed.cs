using LoyalLib.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoyalLib.Db;

public static class Seed
{
    public static async void SeedData(this IApplicationBuilder app)
    {
        using (IServiceScope scope = app.ApplicationServices.CreateScope())
        {
            IServiceProvider provider = scope.ServiceProvider;
            
            DatabaseContext context = provider.GetRequiredService<DatabaseContext>();
            RoleManager<RoleEntity> roleManager = provider.GetRequiredService<RoleManager<RoleEntity>>();
            UserManager<UserEntity> userManager = provider.GetRequiredService<UserManager<UserEntity>>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            UserEntity admin = await CreateUser(userManager,"admin","admin@loyallib.com","zaq1@WSX");
            UserEntity user = await CreateUser(userManager,"user","user@loyallib.com","zaq1@WSX");

            
            await AssignUserToRole(userManager,roleManager, admin, "admin");
            await AssignUserToRole(userManager,roleManager, user, "user");
            
            await context.Books.AddRangeAsync(await GetDataFromWeb());
            await context.SaveChangesAsync();

            List<BookEntity> books = await context.Books.OrderBy(b => b.Id).ToListAsync();

            ChallengeEntity chal0 = new ChallengeEntity()
            {
                Name = "Challenge 0",
                RequiredBooks = new List<BookEntity>([books[0], books[1], books[2]]),
                Points = 140,
                User = admin,
            };
            
            ChallengeEntity chal1 = CreateChallenge(books, "Challenge 1", 40, admin);
            ChallengeEntity chal2 = CreateChallenge(books, "Challenge 2", 25, admin);
            ChallengeEntity chal3 = CreateChallenge(books, "Challenge 3", 15, admin);
            ChallengeEntity chal4 = CreateChallenge(books, "Challenge 4", 20, admin);
            ChallengeEntity chal5 = CreateChallenge(books, "Challenge 5", 80, admin);
            
            ChallengeEntity chal6 = CreateChallenge(books, "Challenge 6", 80, user);

            await context.Challeges.AddRangeAsync( chal0,chal1, chal2, chal3, chal4, chal5, chal6);
            await context.SaveChangesAsync();


        }
    }

    private static ChallengeEntity CreateChallenge(List<BookEntity> books, string challengeName, int points, UserEntity user)
    {
        List<BookEntity> bookEntities = GetRandomBooks(books);
        
        return new ChallengeEntity()
        {
            Name = challengeName,
            RequiredBooks = GetRandomBooks(books),
            Points = points,
            User = user,
        };
    }

    private static List<BookEntity> GetRandomBooks(List<BookEntity> bookEntities)
    {
        Random randomizeBookNumber = new Random();
        List<int> alreadyRandomizedNumbers = new List<int>();
        
        return Enumerable
            .Range(1, randomizeBookNumber.Next(1, 5))
            .Select(i => 
                bookEntities[GetRandomNumber(alreadyRandomizedNumbers,0,bookEntities.Count() - 1)])
            .ToList();
    }

    private static int GetRandomNumber(List<int> alreadyRandomizedNumbers, int min, int max)
    {
        Random random = new Random();
        int number;
        do
        {
            number = random.Next(min, max);
        } while (alreadyRandomizedNumbers.Contains(number));

        return number;
    }

    private static async Task<IEnumerable<BookEntity>> GetDataFromWeb()
    {
        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        
        HttpResponseMessage result = await client.GetAsync("https://wolnelektury.pl/api/books/");
        IAsyncEnumerable<BookEntity?> data = result.Content.ReadFromJsonAsAsyncEnumerable<BookEntity>();
        
        return data.ToBlockingEnumerable()!;
    }
    
    private static async Task<UserEntity> CreateUser(UserManager<UserEntity> userManager, string userName, string email, string password)
    {
        UserEntity user = new UserEntity();
        user.EmailConfirmed = true;
        user.UserName = userName;
        user.Email = email;

        await userManager.CreateAsync(user, password);
        return user;
    }

    private static async Task AssignUserToRole(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, UserEntity user, string roleName)
    {
        RoleEntity role = new RoleEntity();
        role.Name = roleName;
            
        await roleManager.CreateAsync(role);
        await userManager.AddToRoleAsync(user, roleName);
    }
}