using HackerNewsCommentsFeed.Controllers;

namespace HackerNewsCommentsFeed.Configuration;

public static class EndpointMappings
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapItemsEndpoints()
            .MapAuthEndpoints()
            .MapUsersEndpoints()
            .MapInterestsEndpoints();
    }
}