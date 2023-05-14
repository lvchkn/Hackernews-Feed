using Application.Services;
using Application.Services.Stories;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class StoriesController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {   
        app.MapGet("/api/stories", async (
            [FromQuery] SortField? sortBy, 
            [FromQuery] SortOrder? order, 
            [FromServices] IStoriesService storiesService,
            [FromServices] ISorter sorter) =>
        {
            var stories = await storiesService.GetAllAsync();
            var sortedStories = sorter.Sort(stories, new SortingParameters(order ?? SortOrder.Asc, sortBy ?? SortField.None));
            
            return Results.Ok(sortedStories);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Stories);

        return app;
    }
}