using Application.Stories;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class StoriesController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {   
        app.MapGet("/api/stories", (
            [FromQuery] string? orderBy,
            [FromQuery] string? search,
            [FromServices] IStoriesService storiesService) =>
        {
            var stories = storiesService.GetStories(orderBy, search);

            return Results.Ok(stories);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Stories);

        return app;
    }
}