using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HackerNewsCommentsFeed.Domain;

public record User
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonIgnoreIfDefault]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public DateTime LastActive { get; init; } 
}