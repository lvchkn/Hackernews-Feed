using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HackerNewsCommentsFeed.Domain;

public record Interest
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonIgnoreIfDefault]
    [JsonIgnore]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; } = default!;

    public string Text { get; init; } = default!;
}