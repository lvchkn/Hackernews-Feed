using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HackerNewsCommentsFeed.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UsersRepository(IOptions<MongoSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.FeedDatabaseName);
        _usersCollection = database.GetCollection<User>(mongoSettings.Value.UsersCollectionName);
    }
    
    public async Task AddUserAsync(User user)
    {
        await _usersCollection.ReplaceOneAsync( u => u.Id == user.Name, user, new ReplaceOptions {IsUpsert = true});
    }
    
    public async Task UpdateLastActiveAsync(string? email)
    {
        var filter = Builders<User>.Filter.Eq(f => f.Email, email);
        var update = Builders<User>.Update.Set(f => f.LastActive, DateTime.Now);
        await _usersCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>{ReturnDocument = ReturnDocument.After});
    }
        
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await _usersCollection.Find(_ => true).ToListAsync();
        return users;
    }
}