using Application.ApiConnections;
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
        
        app.MapGet("/comments/{id:int}", async ([FromRoute] int id, [FromServices] IApiConnector apiConnector) =>
        {
            var item = await apiConnector.GetComment(id);
    
            return item is null ? Results.NotFound() : Results.Ok(item);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        app.MapGet("/comments/max", async ([FromServices] IApiConnector apiConnector) =>
        {
            var item = await apiConnector.GetLastComment();

            return item is null ? Results.NotFound() : Results.Ok(item);
    
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);
        
        app.MapGet("/comments", async (
            [FromQuery] SortField? sortBy, 
            [FromQuery] SortOrder? order, 
            [FromServices] ICommentsService commentsService,
            [FromServices] ISorter sorter) =>
        {
            var comments = await commentsService.GetAllAsync();
            var sortedComments = sorter.Sort(comments, new SortingParameters(order ?? SortOrder.Asc, sortBy ?? SortField.None));
            
            return Results.Ok(sortedComments);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        app.MapPost("/comments", async ([FromBody] CommentDto comment, [FromServices] ICommentsService commentsService) =>
        {
            await commentsService.AddAsync(comment);

            return Results.NoContent();
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        return app;
    }
}