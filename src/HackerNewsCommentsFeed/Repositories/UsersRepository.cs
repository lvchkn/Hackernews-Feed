using HackerNewsCommentsFeed.Configuration;
using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HackerNewsCommentsFeed.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Interest> _interestsCollection;

    public UsersRepository(IOptions<MongoSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.FeedDatabaseName);
        _usersCollection = database.GetCollection<User>(mongoSettings.Value.UsersCollectionName);
        _interestsCollection = database.GetCollection<Interest>(mongoSettings.Value.InterestsCollectionName);
    }

    private async Task ThrowIfEmailIsInUseAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        
        if (user is not null)
        {
            throw new AlreadyExistsException("Email is already in use.");
        }
    }
    
    private async Task ThrowIfNotFoundAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this email.");
        }
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        var users = await _usersCollection.FindAsync(_ => true);
        
        return await users.ToListAsync();
    }
    
    public async Task<User> GetByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);

        var users = await _usersCollection.FindAsync(filter);
        var user = await users.SingleAsync();

        return user;  
    }

    public async Task<string> AddAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Name, user.Name);

        var res = await _usersCollection.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
        
        return res.UpsertedId?.ToString() ?? "Error";
    }

    public async Task<string> UpdateLastActiveAsync(string email)
    {
        await ThrowIfNotFoundAsync(email);
        
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var update = Builders<User>.Update.Set(u => u.LastActive, DateTime.Now);
        
        var updatedUser = await _usersCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>{ReturnDocument = ReturnDocument.After});
        
        return updatedUser.Id ?? "Error";
    }
    
    public async Task<string> UpdateInterestsAsync(string email, IEnumerable<string> interestIds)
    {
        await ThrowIfNotFoundAsync(email);
        
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var update = Builders<User>.Update.Set(u => u.InterestIds, interestIds);
        
        var updatedUser = await _usersCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>{ReturnDocument = ReturnDocument.After});
        
        return updatedUser.Id ?? "Error";
    }
    
    public async Task<string> AddInterestAsync(string email, Interest interest)
    {
        await ThrowIfEmailIsInUseAsync(email);
        
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var update = Builders<User>.Update.AddToSet(u => u.InterestIds, interest.Id);
        
        var updatedUser = await _usersCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>{ReturnDocument = ReturnDocument.After});
        
        return updatedUser.Id ?? "Error";
    }
    
    public async Task<string> DeleteInterestAsync(string email, string interestId)
    {
        await ThrowIfNotFoundAsync(email);
        
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var update = Builders<User>.Update.Pull(u => u.InterestIds, interestId);
        
        var updatedUser = await _usersCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User>{ReturnDocument = ReturnDocument.After});
        
        return updatedUser.Id ?? "Error";
    }

    public async Task<List<string>> GetInterestsAsync(string email)
    {
        await ThrowIfNotFoundAsync(email);

        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var users = await _usersCollection.FindAsync(filter);
        var user = await users.SingleAsync();

        return user.InterestIds.ToList();
    }

    public async Task<List<string>> GetInterestsNamesAsync(string email)
    {
        var interests = await GetInterestsAsync(email);

        var names = interests.Select(async id =>
        {
            var filter = Builders<Interest>.Filter.Eq(u => u.Id, id);

            var names = await _interestsCollection.FindAsync(filter);
            var name = await names.SingleAsync();

            return name.Text;
        });
        
        var readableNames = await Task.WhenAll(names);

        return readableNames.ToList();
    }
}