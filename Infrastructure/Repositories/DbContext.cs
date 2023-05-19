using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public void SeedData()
    {
        Stories.RemoveRange(Stories);
        
        Stories?.AddRange(
            new Story()
            {
                By = "User1",
                Title = "E Story1",
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
                Title = "A Story1",
                Score = 79,
                Time = 12,
            },
            new Story()
            {
                By = "User4",
                Title = "C Story1",
                Score = 900,
                Time = 4,
            },
            new Story()
            {
                By = "User5",
                Title = "B Story1",
                Score = 250,
                Time = 9,
            }
        );

        SaveChanges();
    }
}