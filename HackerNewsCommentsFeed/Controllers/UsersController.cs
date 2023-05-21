using Application.Interests;
using Application.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class UsersController
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async ([FromServices] IUsersService usersService) =>
        {
            var users = await usersService.GetAllAsync();

            return Results.Ok(users);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Users);
        
        app.MapGet("/api/user/{email}/interests", async (
            [FromRoute] string email, 
            [FromServices] IUsersService usersService) =>
        {
            var userInterests = await usersService.GetInterestsAsync(email);

            return Results.Ok(userInterests);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapPost("/api/user/{email}/interests", async (
            [FromRoute] string email,
            [FromBody] InterestDto interest, 
            [FromServices] IUsersService usersService) =>
        {
            await usersService.AddInterestAsync(email, interest);

            return Results.Ok();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapDelete("/api/user/{email}/interests/{id}", async (
            [FromRoute] string email, 
            [FromRoute] int id,
            [FromServices] IUsersService usersService) =>
        {
            await usersService.DeleteInterestAsync(email, id);

            return Results.NoContent();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        return app;
    }
}