using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Utils;

namespace Infrastructure.Mongo.Repositories;

public class InterestsRepository : IInterestsRepository
{
    private readonly IMongoCollection<Interest> _interestsCollection;

    public InterestsRepository(IOptions<MongoSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.FeedDatabaseName);
        _interestsCollection = database.GetCollection<Interest>(mongoSettings.Value.InterestsCollectionName);
    }

    private async Task ThrowIfNotFoundAsync(string id) //TODO: Remove code duplication across repositories
    {
        var interest = await GetByNameAsync(id);

        if (interest is null)
        {
            throw new NotFoundException("No interest found with this id.");
        }
    }

    public async Task<Interest> GetByNameAsync(string name)
    {
        var filter = Builders<Interest>.Filter.Eq(i => i.Text, name);

        var interests = await _interestsCollection.FindAsync(filter);
        var interest = await interests.SingleAsync();

        return interest;
    }

    public async Task<List<Interest>> GetAllAsync()
    {
        var interests = await _interestsCollection.FindAsync(_ => true);

        return await interests.ToListAsync();
    }

    public async Task<string> AddAsync(Interest interest)
    {
        await _interestsCollection.InsertOneAsync(interest);

        return interest.Id ?? "Error";
    }

    public async Task<string> UpdateAsync(string id, Interest updatedInterest)
    {
        await ThrowIfNotFoundAsync(id);

        var filter = Builders<Interest>.Filter.Eq(i => i.Id, id);
        var updatedInterestResult = await _interestsCollection.FindOneAndReplaceAsync(filter, updatedInterest, new FindOneAndReplaceOptions<Interest> { ReturnDocument = ReturnDocument.After });

        return updatedInterestResult.Id ?? "Error";
    }

    public async Task<string> DeleteAsync(string id)
    {
        await ThrowIfNotFoundAsync(id);

        var filter = Builders<Interest>.Filter.Eq(i => i.Id, id);
        var deletedInterest = await _interestsCollection.FindOneAndDeleteAsync(filter);

        return deletedInterest.Id ?? "Error";
    }
}