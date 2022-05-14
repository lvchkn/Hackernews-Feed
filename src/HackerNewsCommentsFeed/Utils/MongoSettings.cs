namespace HackerNewsCommentsFeed.Utils;

public record MongoSettings
{
    public string ConnectionString { get; init; } = default!;
    public string FeedDatabaseName { get; init; } = default!;
    public string CommentsCollectionName { get; init; } = default!;
    public string UsersCollectionName { get; init; } = default!;

}