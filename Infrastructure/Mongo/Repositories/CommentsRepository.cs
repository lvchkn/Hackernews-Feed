using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly IMongoCollection<Comment> _commentsCollection;

    public CommentsRepository(IOptions<MongoSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.FeedDatabaseName);
        _commentsCollection = database.GetCollection<Comment>(mongoSettings.Value.CommentsCollectionName);
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        var comments = await _commentsCollection.FindAsync(_ => true);
        return await comments.ToListAsync();
    }

    public async Task AddAsync(Comment comment)
    {
        var filter = Builders<Comment>.Filter.Eq(c => c.Id, comment.Id);

        await _commentsCollection.ReplaceOneAsync(filter, comment, new ReplaceOptions { IsUpsert = true });
    }
}