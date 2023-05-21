using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

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
}