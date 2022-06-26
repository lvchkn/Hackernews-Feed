using HackerNewsCommentsFeed.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsCommentsFeed.Controllers;

public static class UsersController
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([FromServices] IUsersRepository usersRepository) =>
        {
            var users = await usersRepository.GetAllAsync();

            return Results.Ok(users);
            
        }).RequireAuthorization().WithTags("Users");
        
        app.MapMethods("/users", new [] { "patch" }, async ([FromServices] IUsersRepository usersRepository) =>
        {
            var users = await usersRepository.GetAllAsync();

            return Results.Ok(users);
            
        }).RequireAuthorization().WithTags("Users");
        
        return app;
    }
}