using Application.Stories;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace Api.Controllers;

public static class StoriesController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/stories", async (
            [FromQuery] string? orderBy,
            [FromQuery] string? search,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromServices] IStoriesService storiesService) =>
        {
            var stories = await storiesService.GetPagedAsync(orderBy, search, pageNumber ?? 1, pageSize ?? 10);

            return Results.Ok(stories);

        }).WithTags(EndpointGroupTags.Stories);

        app.MapGet("/api/stories/{id}/tags", async (
            [FromRoute] int id,
            [FromServices] IStoriesService storiesService) =>
        {
            var tags = await storiesService.GetTagsAsync(id);

            return Results.Ok(tags);

        }).WithTags(EndpointGroupTags.Stories, EndpointGroupTags.Tags);

        return app;
    }
}