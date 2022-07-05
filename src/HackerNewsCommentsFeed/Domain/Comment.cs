using System.Text.Json.Serialization;
using Application.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HackerNewsCommentsFeed.Domain;

public record Comment : IMessage
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonIgnoreIfDefault]
    [JsonIgnore]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ObjectId { get; init; }
    public int Id { get; init; }
    public string By { get; init; }
    public int[] Kids { get; init; }
    public int Parent { get; init; }
    public string Text { get; init; }
    public string Type { get; init; }
    public int Time { get; init; }
}