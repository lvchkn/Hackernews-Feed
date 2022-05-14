using HackerNewsCommentsFeed.Repositories;

namespace HackerNewsCommentsFeed.Controllers;

public static class UsersController
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (IUsersRepository usersRepository) =>
        {
            var users = await usersRepository.GetUsersAsync();

            return Results.Ok(users.ToList());
            
        }).WithTags("Users");
        
        return app;
    }
}