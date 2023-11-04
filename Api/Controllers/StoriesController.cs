using Application.Stories;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace Api.Controllers;

public static class StoriesController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {   
        app.MapGet("/api/stories", (
            [FromQuery] string? orderBy,
            [FromQuery] string? search,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromServices] IStoriesService storiesService) =>
        {
            var stories = storiesService.GetStories(orderBy, search, pageNumber ?? 1, pageSize ?? 10);

            return Results.Ok(stories);
            
        }).WithTags(EndpointGroupTags.Stories);

        return app;
    }
}