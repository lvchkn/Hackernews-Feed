using Application.Tags;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace Api.Controllers;

public static class TagsController
{
    public static IEndpointRouteBuilder MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", async ([FromServices] ITagsService tagsService,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize) =>
        {
            var tags = await tagsService.GetPagedAsync(pageNumber ?? 1, pageSize ?? 10);

            return Results.Ok(tags);

        }).WithTags(EndpointGroupTags.Tags);

        return app;
    }
}