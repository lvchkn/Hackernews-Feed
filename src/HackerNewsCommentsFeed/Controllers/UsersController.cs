using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Repositories;
using HackerNewsCommentsFeed.Utils;
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
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Users);
        
        app.MapGet("/user/{email}/interests", async (
            [FromRoute] string email, 
            [FromServices] IUsersRepository usersRepository) =>
        {
            var users = await usersRepository.GetInterestsNamesAsync(email);

            return Results.Ok(users);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapPost("/user/{email}/interests", async (
            [FromRoute] string email,
            [FromBody] Interest interest, 
            [FromServices] IUsersRepository usersRepository) =>
        {
            var users = await usersRepository.AddInterestAsync(email, interest);

            return Results.Created($"/user/{email}/interests", users);

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapDelete("/user/{email}/interests/{id}", async (
            [FromRoute] string email, 
            [FromRoute] string id,
            [FromServices] IUsersRepository usersRepository) =>
        {
            var users = await usersRepository.DeleteInterestAsync(email, id);

            return Results.Ok(users);

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        return app;
    }
}