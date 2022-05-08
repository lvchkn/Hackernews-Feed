using HackerNewsCommentsFeed.ApiConnections;

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

        return app;
    }
}