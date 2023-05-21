using Domain.Entities;
using Infrastructure.Db;

namespace Tests.Integration;

public static class DataSeeder
{
    public static void SeedStories(this AppDbContext dbContext)
    {
        dbContext.Stories.RemoveRange(dbContext.Stories);
        
        dbContext.Stories?.AddRange(
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
            }
        );

        dbContext.SaveChanges();
    }
}