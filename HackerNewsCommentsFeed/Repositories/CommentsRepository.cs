using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HackerNewsCommentsFeed.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly IMongoCollection<Comment> _commentsCollection;

    public CommentsRepository(IOptions<MongoSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.FeedDatabaseName);
        _commentsCollection = database.GetCollection<Comment>(mongoSettings.Value.CommentsCollectionName);
    }
    
    public async Task<IEnumerable<Comment>> GetCommentsAsync()
    {
        var comments = await _commentsCollection.Find(_ => true).ToListAsync();
        return comments;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _commentsCollection.ReplaceOneAsync( c => c.Id == comment.Id, comment, new ReplaceOptions {IsUpsert = true});
    }
}