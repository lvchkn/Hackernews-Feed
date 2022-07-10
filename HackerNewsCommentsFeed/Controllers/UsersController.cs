using Application.Contracts;
using Application.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class UsersController
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([FromServices] IUsersService usersService) =>
        {
            var users = await usersService.GetAllAsync();

            return Results.Ok(users);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Users);
        
        app.MapGet("/user/{email}/interests", async (
            [FromRoute] string email, 
            [FromServices] IUsersService usersService) =>
        {
            var users = await usersService.GetInterestsNamesAsync(email);

            return Results.Ok(users);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapPost("/user/{email}/interests", async (
            [FromRoute] string email,
            [FromBody] InterestDto interest, 
            [FromServices] IUsersService usersService) =>
        {
            var users = await usersService.AddInterestAsync(email, interest);

            return Results.Created($"/user/{email}/interests", users);

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapDelete("/user/{email}/interests/{id}", async (
            [FromRoute] string email, 
            [FromRoute] string id,
            [FromServices] IUsersService usersService) =>
        {
            var users = await usersService.DeleteInterestAsync(email, id);

            return Results.Ok(users);

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        return app;
    }
}