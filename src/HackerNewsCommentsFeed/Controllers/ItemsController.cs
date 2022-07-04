using HackerNewsCommentsFeed.ApiConnections;
using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Repositories;
using HackerNewsCommentsFeed.Utils;
using Microsoft.AspNetCore.Mvc;

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
            [FromServices] ICommentsRepository commentsRepository,
            [FromServices] Sorter sorter) =>
        {
            var comments = await commentsRepository.GetAllAsync();
            var sortedComments = sorter.Sort(comments, new SortingParameters(order ?? SortOrder.Asc, sortBy ?? SortField.None));
            
            return Results.Ok(sortedComments);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        app.MapPost("/comments", async ([FromBody] Comment comment, [FromServices] ICommentsRepository commentsRepository) =>
        {
            await commentsRepository.AddAsync(comment);

            return Results.NoContent();
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Comments);

        return app;
    }
}