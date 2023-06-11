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
        
        app.MapGet("/api/user/{id}/interests", async (
            [FromRoute] int id, 
            [FromServices] IUsersService usersService) =>
        {
            var userInterests = await usersService.GetInterestsAsync(id);

            return Results.Ok(userInterests);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapPost("/api/user/{id}/interests", async (
            [FromRoute] int id,
            [FromBody] InterestDto interest, 
            [FromServices] IUsersService usersService) =>
        {
            await usersService.AddInterestAsync(id, interest);

            return Results.Ok();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapDelete("/api/user/{userId}/interests/{interestId}", async (
            [FromRoute] int userId, 
            [FromRoute] int interestId,
            [FromServices] IUsersService usersService) =>
        {
            await usersService.DeleteInterestAsync(userId, interestId);

            return Results.NoContent();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        return app;
    }
}