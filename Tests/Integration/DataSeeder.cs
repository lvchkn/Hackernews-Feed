using Domain.Entities;
using Infrastructure.Db;

namespace Tests.Integration;

public static class DataSeeder
{
    public static int SeedTags(this AppDbContext dbContext)
    {
        dbContext.Tags.RemoveRange(dbContext.Tags);

        var tags = new[]
        {
            new Tag
            {
                Name = ".NET",
                Aliases = [
                    new TagAlias { Name = "dotnet" },
                    new TagAlias { Name = "dotnet core" },
                    new TagAlias { Name = ".NET Core" },
                    new TagAlias { Name = "Dotnet" },
                ],
                Category = "Frameworks",
            },
            new Tag
            {
                Name = "PostgreSQL",
                Aliases = [
                    new TagAlias { Name = "Postgres" },
                    new TagAlias { Name = "psql" },
                ],
                Category = "Databases",
            },
            new Tag
            {
                Name = "Redis",
                Aliases = [],
                Category = "Databases",
            },
            new Tag
            {
                Name = "Kubernetes",
                Aliases = [
                    new TagAlias { Name = "K8s" },
                ],
                Category = "Programming Tools",
            },
            new Tag
            {
                Name = "Docker",
                Aliases = [],
                Category = "Programming Tools",
            },
        };

        dbContext.Tags.AddRange(tags);
        dbContext.SaveChanges();

        return tags.Length;
    }

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

        dbContext.Stories.AddRange(stories);

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
            new() { Text = ".NET Core" },
            new() { Text = "AWS" },
            new() { Text = "finance" },
        };

        var stories = new List<Story>()
        {
            new()
            {
                By = "someone",
                Title = "Hello World!",
                Score = 123,
                Time = 7000,
            },
            new()
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