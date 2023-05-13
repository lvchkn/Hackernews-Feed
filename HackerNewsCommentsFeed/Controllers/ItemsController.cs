using Application.Contracts;
using Application.Services;
using Application.Services.Comments;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class ItemsController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {   
        app.MapGet("/api/comments", async (
            [FromQuery] SortField? sortBy, 
            [FromQuery] SortOrder? order, 
            [FromServices] ICommentsService commentsService,
            [FromServices] ISorter sorter) =>
        {
            var comments = await commentsService.GetAllAsync();
            var sortedComments = sorter.Sort(comments, new SortingParameters(order ?? SortOrder.Asc, sortBy ?? SortField.None));
            
            return Results.Ok(sortedComments);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        app.MapPost("/api/comments", async ([FromBody] CommentDto comment, [FromServices] ICommentsService commentsService) =>
        {
            await commentsService.AddAsync(comment);

            return Results.Ok();
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        return app;
    }
}