using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories;

public class StoriesRepository : IStoriesRepository
{
    private readonly IMongoCollection<Story> _storiesCollection;

    public StoriesRepository(IOptions<MongoSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.FeedDatabaseName);
        _storiesCollection = database.GetCollection<Story>(mongoSettings.Value.StoriesCollectionName);
    }

    public async Task<string> AddAsync(Story story)
    {
        var filter = Builders<Story>.Filter.Eq(s => s.Id, story.Id);

        var upsertResult = await _storiesCollection.ReplaceOneAsync(filter, story, new ReplaceOptions { IsUpsert = true });

        return upsertResult.UpsertedId.AsString ?? "Error";
    }
}