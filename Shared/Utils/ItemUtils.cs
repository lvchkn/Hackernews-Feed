using System.Text.Json;

namespace Shared.Utils;

public static class ItemUtils
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private record ItemType
    {
        public string? Type { get; init; }
    }

    public static string? GetItemType(string response)
    {
        var itemType = JsonSerializer.Deserialize<ItemType>(response, _jsonSerializerOptions);
        return itemType?.Type;
    }
}