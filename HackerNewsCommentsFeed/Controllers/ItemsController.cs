using HackerNewsCommentsFeed.ApiConnections;
using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsCommentsFeed.Controllers;

public static class ItemsController
{
    public static IEndpointRouteBuilder MapItemsEndpoints(this IEndpointRouteBuilder app)
    {
        
        app.MapGet("/{id:int}", async (int id, IApiConnector apiConnector) =>
        {
            var item = await apiConnector.GetComment(id);
    
            return item is null ? Results.NotFound() : Results.Ok(item);
            
        }).WithTags("Comments");

        app.MapGet("/max", async (IApiConnector apiConnector) =>
        {
            var item = await apiConnector.GetLastComment();

            return item is null ? Results.NotFound() : Results.Ok(item);
    
        }).RequireAuthorization().WithTags("Comments");
        
        app.MapGet("/saved", async (ICommentsRepository commentsRepository) =>
        {
            var comments = await commentsRepository.GetCommentsAsync();

            return Results.Ok(comments.ToList());
            
        }).WithTags("Comments");

        app.MapPost("/add", async ([FromBody] Comment comment, ICommentsRepository commentsRepository) =>
        {
            await commentsRepository.AddCommentAsync(comment);

            return Results.NoContent();
            
        }).WithTags("Comments");

        return app;
    }
}