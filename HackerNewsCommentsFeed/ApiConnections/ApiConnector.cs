using System.Text.Json;
using System.Web;
using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.RabbitConnections.Publisher;
using HackerNewsCommentsFeed.Utils;

namespace HackerNewsCommentsFeed.ApiConnections;

public class ApiConnector : IApiConnector
{
    private readonly ILogger<ApiConnector> _logger;
    private readonly IPublisher _publisher;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    private const string ApiUrlItem = "item/{0}.json";
    private const string ApiUrlMaxItem = "maxitem.json";

    public ApiConnector(IHttpClientFactory httpClientFactory, ILogger<ApiConnector> logger, IPublisher publisher)   
    {
        _logger = logger;
        _publisher = publisher;
        _httpClient = httpClientFactory.CreateClient("ApiV0");
    }
    
    public async Task<Comment?> GetComment(int id)
    {
        var response = await _httpClient.GetAsync(string.Format(ApiUrlItem, id.ToString()));
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.Log(LogLevel.Warning, "Response: {0}", responseString);
        
        var itemType = ItemUtils.GetItemType(responseString);

        if (itemType?.ToLower() != "comment")
        {
            return null;
        }
        
        var comment = JsonSerializer.Deserialize<Comment>(responseString, _jsonSerializerOptions);

        if (comment?.Text is not null)
        {
            var unescapedText = HttpUtility.HtmlDecode(comment.Text);
            _logger.Log(LogLevel.Information, "Unescaped string: {0}", unescapedText);
            
            _publisher.Publish("Feed", comment);
        }
        
        _logger.Log(LogLevel.Information, "Comment has been received: {0}", comment);

        return comment;
    }

    public async Task<Comment?> GetLastComment()
    {
        var response = await _httpClient.GetAsync(ApiUrlMaxItem);
        var responseString = await response.Content.ReadAsStringAsync();
        
        var id = JsonSerializer.Deserialize<int>(responseString);
        Comment? comment;
        var maxRetries = 100;

        do
        {
            comment = await GetComment(id);
            id--;
            maxRetries--;

        } while (comment is null && id > 1 && maxRetries > 0);
        
        return comment;
    }
}