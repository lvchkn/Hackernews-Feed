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
        });

        app.MapGet("/max", async (IApiConnector apiConnector) =>
        {
            var item = await apiConnector.GetLastComment();

            return item is null ? Results.NotFound() : Results.Ok(item);
    
        }).RequireAuthorization();
        
        app.MapGet("/saved", async (ICommentsRepository commentsRepository) =>
        {
            var comments = await commentsRepository.GetCommentsAsync();

            return Results.Ok(comments.ToList());
        });

        app.MapPost("/add", async ([FromBody] Comment comment, ICommentsRepository commentsRepository) =>
        {
            await commentsRepository.AddCommentAsync(comment);

            return Results.NoContent();
        });

        return app;
    }
}