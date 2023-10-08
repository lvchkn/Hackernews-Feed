using Domain.Entities;
using Infrastructure.Db;

namespace Tests.Integration;

public static class DataSeeder
{
    public static int SeedStories(this AppDbContext dbContext)
    {
        dbContext.Stories.RemoveRange(dbContext.Stories);

        var stories = new []
        {
            new Story()
            {
                By = "User1",
                Title = "E Story",
                Score = 25,
                Time = 1,
            },
            new Story()
            {
                By = "User2",
                Title = "B Story",
                Score = 44,
                Time = 2,
            },
            new Story()
            {
                By = "User3",
                Title = "A Story",
                Score = 79,
                Time = 12,
            },
            new Story()
            {
                By = "User4",
                Title = "C Story",
                Score = 900,
                Time = 4,
            },
            new Story()
            {
                By = "User5",
                Title = "B Story",
                Score = 250,
                Time = 9,
            }, 
        };
        
        dbContext.Stories?.AddRange(stories);

        dbContext.SaveChanges();

        return stories.Length;
    }

    public static void SeedUsers(this AppDbContext dbContext)
    {
        dbContext.Users.RemoveRange(dbContext.Users);

        var user = new User()
        {
            Name = "testuser1",
            Email = "test@example.com",
            LastActive = DateTime.UtcNow,
            Interests = new List<Interest>(),
            FavouriteStories = new List<Story>(),
        };

        var interests = new List<Interest>
        {
            new Interest { Text = ".NET Core" },
            new Interest { Text = "AWS" },
            new Interest { Text = "finance" },
        };

        var stories = new List<Story>()
        {
            new Story()
            {
                By = "someone",
                Title = "Hello World!",
                Score = 123,
                Time = 7000,
            },
            new Story()
            {
                By = "testing",
                Title = "how to learn git",
                Score = 200,
                Time = 10000,
            },
        };

        user.Interests.AddRange(interests);
        user.FavouriteStories.AddRange(stories);

        dbContext.Users.Add(user);

        dbContext.SaveChanges();
    }
}