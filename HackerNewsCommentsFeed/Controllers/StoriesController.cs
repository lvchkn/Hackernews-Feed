using Application.Services.Stories;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class StoriesController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {   
        app.MapGet("/api/stories", (
            [FromQuery] string? orderBy, 
            [FromServices] IStoriesService storiesService) =>
        {
            var sortedStories = storiesService.GetSortedStories(orderBy);   
            return Results.Ok(sortedStories);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Stories);

        return app;
    }
}